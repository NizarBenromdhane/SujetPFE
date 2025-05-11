using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SujetPFE.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Linq;
using SujetPFE.Domain.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Models;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SujetPFE.Controllers
{
    public class ObjectivationsController : Controller
    {
        private readonly PcbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public ObjectivationsController(PcbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult SaisieCredit()
        {
            try
            {
                ViewBag.ChargesAffaires = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();

                int? currentEmployeeId = _context.Employees.FirstOrDefault(e => e.Nom == "HB")?.Id;
                var groupesAvecEncours = GetGroupesAvecEncoursCredit(currentEmployeeId);
                return View(groupesAvecEncours);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaisieCredit GET: {ex.Message}");
                ViewBag.ChargesAffaires = new List<SelectListItem>();
                return View(new List<CreditObjectifViewModel>());
            }
        }


        private List<CreditObjectifViewModel> GetGroupesAvecEncoursCredit(int? chargeAffairesId = null)
        {
            IQueryable<Groupe> groupesQuery = _context.Groupes;

            if (chargeAffairesId.HasValue)
            {
                groupesQuery = groupesQuery.Where(g => g.EmployeResponsableId == chargeAffairesId.Value);
            }

            var result = groupesQuery.Select(g => new CreditObjectifViewModel
            {
                GroupeNom = g.Nom,
                GroupeId = g.Id,
                Devise = _context.Encours
        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Crédit")
        .Select(e => e.Devise == "TND" ? "1" :
                        e.Devise == "EUR" ? "2" :
                        e.Devise == "USD" ? "3" : "1")
        .FirstOrDefault() ?? "1",

                Encours2023Dat = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "crédit" && e.Sens == "DAT" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                    .Sum(e => (decimal?)e.Solde),

                Encours2023Dav = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "crédit" && e.Sens == "DAV" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                    .Sum(e => (decimal?)e.Solde),

                Encours2024Dat = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "crédit" && e.Sens == "DAT" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                    .Sum(e => (decimal?)e.Solde),

                Encours2024Dav = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "crédit" && e.Sens == "DAV" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                    .Sum(e => (decimal?)e.Solde),

                Objectif2025Dat = _context.ObjectifsCreditDepots // Assuming the same table for both depot and credit objectives
                    .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Crédit" && o.Annee == 2025)
                    .Select(o => o.MontantDat)
                    .FirstOrDefault(),

                Objectif2025Dav = _context.ObjectifsCreditDepots // Assuming the same table for both depot and credit objectives
                    .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Crédit" && o.Annee == 2025)
                    .Select(o => o.MontantDav)
                    .FirstOrDefault(),

                EmployeResponsableId = g.EmployeResponsableId
            }).ToList();

            foreach (var item in result)
            {
                item.Encours2023Total = (item.Encours2023Dat ?? 0) + (item.Encours2023Dav ?? 0);
                item.Encours2024Total = (item.Encours2024Dat ?? 0) + (item.Encours2024Dav ?? 0);
                item.Objectif2025Total = (item.Objectif2025Dat ?? 0) + (item.Objectif2025Dav ?? 0);
            }

            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnregistrerObjectifs(List<CreditObjectifViewModel> model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { isSuccess = false, message = "Erreurs de validation : ", errors = errors });
            }

            try
            {
                List<string> errors = new List<string>();
                foreach (var item in model)
                {
                    if (!item.GroupeId.HasValue)
                    {
                        Console.WriteLine($"Warning: GroupeId is null for an item. Skipping.");
                        continue;
                    }

                    int groupeId = item.GroupeId.Value;

                    try
                    {
                        var objectif = await _context.ObjectifsCreditDepots
                            .FirstOrDefaultAsync(o => o.GroupeId == groupeId &&
                                                     o.TypeObjectif == "Crédit" &&
                                                     o.Annee == 2025);

                        if (objectif != null)
                        {
                            objectif.MontantDat = item.Objectif2025Dat;
                            objectif.MontantDav = item.Objectif2025Dav;
                            objectif.Montant = (item.Objectif2025Dat ?? 0) + (item.Objectif2025Dav ?? 0);
                            objectif.EmployeId = item.EmployeResponsableId;
                            _context.Entry(objectif).State = EntityState.Modified;
                        }
                        else if ((item.Objectif2025Dat ?? 0) > 0 || (item.Objectif2025Dav ?? 0) > 0)
                        {
                            objectif = new ObjectifCreditDepot
                            {
                                GroupeId = groupeId,
                                TypeObjectif = "Crédit",
                                MontantDat = item.Objectif2025Dat,
                                MontantDav = item.Objectif2025Dav,
                                Montant = (item.Objectif2025Dat ?? 0) + (item.Objectif2025Dav ?? 0),
                                DateDebut = new DateTime(2025, 1, 1),
                                DateFin = new DateTime(2025, 12, 31),
                                EmployeId = item.EmployeResponsableId
                            };
                            await _context.ObjectifsCreditDepots.AddAsync(objectif);
                        }
                        // If both Objectif2025Dat and Objectif2025Dav are 0 and an existing objective is found, remove it.
                        else if (objectif != null)
                        {
                            _context.ObjectifsCreditDepots.Remove(objectif);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing group {groupeId} (Crédit): {ex.Message}");
                        errors.Add($"Erreur lors du traitement du groupe {groupeId}: {ex.Message}");
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    if (errors.Any())
                    {
                        return Json(new { isSuccess = false, message = "Certains objectifs n'ont pas pu être enregistrés : " + string.Join("; ", errors) });
                    }
                    // On success, return a success message and potentially updated data to refresh the UI
                    var updatedData = GetGroupesAvecEncoursCredit(model.FirstOrDefault()?.EmployeResponsableId);
                    return Json(new { isSuccess = true, message = "Les objectifs de crédit ont été enregistrés avec succès.", data = updatedData });
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Database update error: {ex.Message}");
                    return Json(new { isSuccess = false, message = "Erreur lors de l'enregistrement dans la base de données : " + ex.Message });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General error saving changes: {ex.Message}");
                    return Json(new { isSuccess = false, message = "Erreur générale lors de l'enregistrement : " + ex.Message });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Outer error in EnregistrerObjectifs: {ex.Message}");
                return Json(new { isSuccess = false, message = "Une erreur inattendue s'est produite lors de l'enregistrement." });
            }
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SaisieDepot()
        {
            try
            {
                ViewBag.ChargesAffaires = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();

                int? currentEmployeeId = _context.Employees.FirstOrDefault(e => e.Nom == "HB")?.Id;
                var groupesAvecEncours = GetGroupesAvecEncours(currentEmployeeId);
                return View(groupesAvecEncours);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaisieDepot GET: {ex.Message}");
                ViewBag.ChargesAffaires = new List<SelectListItem>();
                return View(new List<ObjectifDepotViewModel>());
            }
        }

        private List<ObjectifDepotViewModel> GetGroupesAvecEncours(int? chargeAffairesId = null)
        {
            IQueryable<Groupe> groupesQuery = _context.Groupes;

            if (chargeAffairesId.HasValue)
            {
                groupesQuery = groupesQuery.Where(g => g.EmployeResponsableId == chargeAffairesId.Value);
            }

            var result = groupesQuery.Select(g => new ObjectifDepotViewModel
            {
                Groupe = g,
                GroupeId = g.Id,
                GroupeNom = g.Nom,
                // Remove Devise or get it from Groupe if available
                Devise = "EUR", // Default value or g.Devise if Groupe has a Devise property

                Encours2023Dat = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAT" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                    .Sum(e => (decimal?)e.Solde),

                Encours2023Dav = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAV" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                    .Sum(e => (decimal?)e.Solde),

                Encours2024Dat = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAT" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                    .Sum(e => (decimal?)e.Solde),

                Encours2024Dav = _context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAV" &&
                                        e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                    .Sum(e => (decimal?)e.Solde),

                Objectif2025 = _context.ObjectifsCreditDepots
                    .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025)
                    .Select(o => new Objectif2025ViewModel
                    {
                        MontantDat = o.MontantDat,
                        MontantDav = o.MontantDav
                    })
                    .FirstOrDefault() ?? new Objectif2025ViewModel(),

                EmployeResponsableId = g.EmployeResponsableId
            }).ToList();

            // Calculate derived properties
            foreach (var item in result)
            {
                item.Encours2023Total = (item.Encours2023Dat ?? 0) + (item.Encours2023Dav ?? 0);
                item.Encours2024Total = (item.Encours2024Dat ?? 0) + (item.Encours2024Dav ?? 0);
                item.Evolution2024 = item.Encours2023Total != 0 ?
                    (item.Encours2024Total - item.Encours2023Total) / item.Encours2023Total * 100 : 0;
                item.Objectif2025Total = item.Objectif2025.TotalDep;
                item.Evolution2025 = item.Encours2024Total != 0 ?
                    (item.Objectif2025Total - item.Encours2024Total) / item.Encours2024Total * 100 : 0;
            }

            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaisieDepot(List<ObjectifDepotViewModel> model)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in model)
                {
                    try
                    {
                        var objectif = _context.ObjectifsCreditDepots
                            .FirstOrDefault(o => o.GroupeId == item.GroupeId &&
                                                 o.TypeObjectif == "Dépôt" &&
                                                 o.Annee == 2025);

                        decimal montantTotal = item.Objectif2025Total; // Use the calculated property

                        if (objectif != null)
                        {
                            objectif.Montant = montantTotal;
                            objectif.EmployeId = item.EmployeResponsableId;
                            objectif.MontantDat = item.Objectif2025.MontantDat;
                            objectif.MontantDav = item.Objectif2025.MontantDav;
                            _context.Entry(objectif).State = EntityState.Modified;
                        }
                        else if (montantTotal > 0)
                        {
                            objectif = new ObjectifCreditDepot
                            {
                                GroupeId = item.GroupeId,
                                TypeObjectif = "Dépôt",
                                Montant = montantTotal,
                                MontantDat = item.Objectif2025.MontantDat,
                                MontantDav = item.Objectif2025.MontantDav,
                                DateDebut = new DateTime(2025, 1, 1),
                                DateFin = new DateTime(2025, 12, 31),
                                EmployeId = item.EmployeResponsableId
                            };
                            _context.ObjectifsCreditDepots.Add(objectif);
                        }
                        //  else if (montantTotal == 0)
                        // {
                        //     //Si le montant total est 0, on supprime l'objectif s'il existe déjà
                        //     if (objectif != null)
                        //     {
                        //         _context.ObjectifsCreditDepots.Remove(objectif);
                        //     }
                        // }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing group {item.GroupeId}: {ex.Message}");
                        TempData["Message"] = "Erreur lors de l'enregistrement de certains objectifs.";
                        TempData["IsSuccess"] = false;
                        return View(model);
                    }
                }

                try
                {
                    _context.SaveChanges();
                    TempData["Message"] = "Les objectifs de dépôt ont été enregistrés avec succès.";
                    TempData["IsSuccess"] = true;
                    return RedirectToAction("Dashboard");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving changes: {ex.Message}");
                    TempData["Message"] = "Erreur lors de l'enregistrement : " + ex.Message;
                    TempData["IsSuccess"] = false;
                }
            }

            // Repopulate ViewBag if ModelState is invalid or an error occurred
            try
            {
                ViewBag.ChargesAffaires = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees for post-back: {ex.Message}");
                ViewBag.ChargesAffaires = new List<SelectListItem>();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreditDashboard()
        {
            // *** DONNÉES DE TEST POUR L'ENCOURS PAR GROUPE ***
            var encoursTest = new List<dynamic>
    {
        new { Nom = "Groupe A", Encours2023 = 150000.00m, Encours2024 = 165000.00m },
        new { Nom = "Groupe B", Encours2023 = 220000.00m, Encours2024 = 200000.00m },
        new { Nom = "Groupe C", Encours2023 = 90000.00m, Encours2024 = 100000.00m }
    };
            ViewBag.CreditGroupData = encoursTest;

            // *** DONNÉES DE TEST POUR LES OBJECTIFS DE CRÉDIT ***
            var objectifsTest = new List<CreditObjectif>
    {
        new CreditObjectif { Periode = "Mars 2025", TypeCredit = "Personnel", MontantObjectif = 50000.00m, EmployeId = 1, GroupeId = 1 },
        new CreditObjectif { Periode = "Avril 2025", TypeCredit = "Immobilier", MontantObjectif = 120000.00m, EmployeId = 2, GroupeId = 2 },
        new CreditObjectif { Periode = "Avril 2025", TypeCredit = "Auto", MontantObjectif = 35000.00m, EmployeId = 1, GroupeId = 1 }
    };
            ViewBag.CreditData = objectifsTest;

            // *** CALCULS BASÉS SUR LES DONNÉES DE TEST (OU VOS DONNÉES RÉELLES) ***
            ViewBag.TotalCreditObjectives = objectifsTest.Count();
            ViewBag.TotalCreditValue = objectifsTest.Sum(o => o.MontantObjectif);
            ViewBag.AverageCreditValue = objectifsTest.Any() ? objectifsTest.Average(o => o.MontantObjectif) : 0;

            // *** POPULATE VIEW BAG FOR EMPLOYEES AND GROUPS ***
            ViewBag.Employes = new SelectList(await _context.Employees.ToListAsync(), "Id", "NomComplet");
            ViewBag.Groupes = new SelectList(await _context.Groupes.ToListAsync(), "Id", "Nom");

            return View();
        }

        [HttpGet]
        public IActionResult DepotDashboard()
        {
            try
            {
                var depotData = _context.ObjectifsCreditDepots
                    .Where(o => o.TypeObjectif == "Dépôt")
                    .ToList();

                ViewBag.TotalDepotObjectifs = depotData.Count();
                ViewBag.TotalDepotValue = depotData.Sum(o => o.Montant);
                ViewBag.AverageDepotValue = depotData.Any() ? depotData.Average(o => o.Montant) : 0;
                ViewBag.DepotData = depotData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DepotDashboard: {ex.Message}");
                ViewBag.TotalDepotObjectifs = 0;
                ViewBag.TotalDepotValue = 0;
                ViewBag.DepotData = new List<ObjectifCreditDepot>();
            }
            return View();
        }
        [HttpGet]
        public IActionResult ModifierCredit(int? id)
        {
            var creditObjectif = new CreditObjectif { Id = id ?? 0 };

            // Fetch employees for the EmployeId dropdown
            var employes = _context.Employees
                .Select(e => new { Id = e.Id, NomComplet = e.Nom + " (" + e.Matricule1 + ")" })
                .OrderBy(e => e.NomComplet)
                .ToList();
            ViewBag.Employes = new SelectList(employes, "Id", "NomComplet"); // Changed ViewBag name

            // Fetch groups for the GroupeId dropdown
            var groupes = _context.Groupes
                .Select(g => new { Id = g.Id, NomGroupe = g.Nom + " (" + g.IdBct + ")" })
                .OrderBy(g => g.NomGroupe)
                .ToList();
            ViewBag.Groupes = new SelectList(groupes, "Id", "NomGroupe"); // Changed ViewBag name

            return View(creditObjectif);
        }
        [HttpPost("Objectivations/ModifierCredit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult ModifierCredit(int id, [Bind("Id,Periode,TypeCredit,MontantObjectif,EmployeId,GroupeId")] CreditObjectif creditObjectif)
        {
            if (id != creditObjectif.Id)
            {
                return BadRequest();
            }

            // Variable pour indiquer si la redirection doit avoir lieu
            bool shouldRedirect = true;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(creditObjectif);
                    _context.SaveChanges();
                    // Redirection en cas de succès (la variable est déjà à true)
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CreditObjectifExists(creditObjectif.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // On relance l'exception, donc shouldRedirect reste true
                    }
                }
                catch (Exception ex)
                {
                    // Log l'erreur ici
                    Console.WriteLine($"Erreur lors de la sauvegarde de l'objectif de crédit : {ex.Message}");
                    // Optionnel : Vous pouvez stocker un message d'erreur dans ViewBag pour l'afficher sur le CreditDashboard
                    ViewBag.ErrorMessage = "Une erreur s'est produite lors de la tentative d'enregistrement.";
                    // Indiquer de ne pas rediriger en cas d'erreur (si vous le souhaitez)
                    // shouldRedirect = false;
                }
            }
            else
            {
                // Optionnel : Vous pouvez stocker un message d'erreur de validation dans ViewBag
                ViewBag.ErrorMessage = "Veuillez corriger les erreurs de validation.";
                // La redirection aura lieu car shouldRedirect est true par défaut
            }

            // Redirection après le bloc try-catch-finally (ou try-finally)
            return RedirectToAction(nameof(CreditDashboard));
        }
        // Action pour supprimer un objectif de crédit (POST)
        [HttpPost("Objectivations/SupprimerCredit/{id:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult SupprimerCredit(int id)
        {
            if (_context.CreditObjectifs == null)
            {
                return NotFound();
            }
            var creditObjectif = _context.CreditObjectifs.Find(id);
            if (creditObjectif == null)
            {
                return NotFound();
            }

            _context.CreditObjectifs.Remove(creditObjectif);
            _context.SaveChanges();
            return RedirectToAction(nameof(CreditDashboard));
        }

        private bool CreditObjectifExists(int id)
        {
            return _context.CreditObjectifs.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupesData(int? employeId)
        {
            if (!employeId.HasValue)
            {
                return Json(new List<ObjectifDepotViewModel>());
            }

            var groupes = await _context.Groupes
                .Where(g => g.EmployeResponsableId == employeId.Value)
                .ToListAsync();

            var result = new List<ObjectifDepotViewModel>();

            foreach (var g in groupes)
            {
                var objectif = await _context.ObjectifsCreditDepots
                    .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025)
                    .Select(o => new Objectif2025ViewModel
                    {
                        MontantDat = o.MontantDat,
                        MontantDav = o.MontantDav
                    })
                    .FirstOrDefaultAsync() ?? new Objectif2025ViewModel();

                var viewModel = new ObjectifDepotViewModel
                {
                    Groupe = g,
                    GroupeId = g.Id,
                    GroupeNom = g.Nom,
                    Devise = "EUR", // Default value or get from Groupe if available

                    Encours2023Dat = await _context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAT" &&
                                            e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                        .SumAsync(e => (decimal?)e.Solde) ?? 0,

                    Encours2023Dav = await _context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAV" &&
                                            e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                        .SumAsync(e => (decimal?)e.Solde) ?? 0,

                    Encours2024Dat = await _context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAT" &&
                                            e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                        .SumAsync(e => (decimal?)e.Solde) ?? 0,

                    Encours2024Dav = await _context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "dépôt" && e.Sens == "DAV" &&
                                            e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                        .SumAsync(e => (decimal?)e.Solde) ?? 0,

                    Objectif2025 = objectif,
                    EmployeResponsableId = g.EmployeResponsableId
                };

                // Calculate derived properties
                viewModel.Encours2023Total = (viewModel.Encours2023Dat ?? 0) + (viewModel.Encours2023Dav ?? 0);
                viewModel.Encours2024Total = (viewModel.Encours2024Dat ?? 0) + (viewModel.Encours2024Dav ?? 0);
                viewModel.Evolution2024 = viewModel.Encours2023Total != 0 ?
                    (viewModel.Encours2024Total - viewModel.Encours2023Total) / viewModel.Encours2023Total * 100 : 0;
                viewModel.Objectif2025Total = viewModel.Objectif2025.TotalDep;
                viewModel.Evolution2025 = viewModel.Encours2024Total != 0 ?
                    (viewModel.Objectif2025Total - viewModel.Encours2024Total) / viewModel.Encours2024Total * 100 : 0;

                result.Add(viewModel);
            }

            return Json(result);
        }
    }
}
