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
using System.Threading.Tasks;

namespace SujetPFE.Controllers
{
    public class ObjectivationsController : Controller
    {
        private readonly PcbContext _context;
        private readonly IConfiguration _configuration;

        public ObjectivationsController(PcbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult SaisieCredit()
        {
            try
            {
                ViewBag.Employes = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();

                ViewBag.Groupes = _context.Groupes
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Nom })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaisieCredit GET: {ex.Message}");
                ViewBag.Employes = new List<SelectListItem>();
                ViewBag.Groupes = new List<SelectListItem>();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaisieCredit(List<CreditObjectif> model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.CreditObjectifs.AddRange(model);
                    _context.SaveChanges();

                    ViewBag.Message = "Les objectifs de crédit ont été enregistrés avec succès.";
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving CreditObjectifs: {ex.Message}");
                    ViewBag.Message = "Erreur lors de l'enregistrement des objectifs de crédit.";
                    ViewBag.IsSuccess = false;
                }
            }

            // Repopulate ViewBag if ModelState is invalid or an error occurred
            try
            {
                ViewBag.Employes = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();

                ViewBag.Groupes = _context.Groupes
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Nom })
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error repopulating ViewBag in SaisieCredit POST: {ex.Message}");
                ViewBag.Employes = new List<SelectListItem>();
                ViewBag.Groupes = new List<SelectListItem>();
            }

            return View(model);
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

                        decimal montantTotal = item.Objectif2025.TotalDep;

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
        public IActionResult CreditDashboard()
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