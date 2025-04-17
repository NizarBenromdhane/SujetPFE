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
                var employes = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();
                ViewBag.Employes = employes;

                var groupes = _context.Groupes
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Nom })
                    .ToList();
                ViewBag.Groupes = groupes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaisieCredit GET: {ex.Message}");
                ViewBag.Employes = new List<SelectListItem>();
                ViewBag.Groupes = new List<SelectListItem>();
            }
            return View(); // This line should look for /Views/Objectivations/SaisieCredit.cshtml
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
                    return View(); // Stay on the same page after successful save
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
            List<SelectListItem> employesFromDb = new List<SelectListItem>();
            try
            {
                employesFromDb = _context.Employees
                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                    .ToList();
                ViewBag.ChargesAffaires = employesFromDb;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees: {ex.Message}");
                ViewBag.ChargesAffaires = new List<SelectListItem>();
            }

            // Initially load all groups without filtering
            var groupesAvecEncours = GetAllGroupesAvecEncours();
            return View(groupesAvecEncours);
        }

        private List<ObjectifDepotViewModel> GetAllGroupesAvecEncours(int? chargeAffairesId = null)
        {
            IQueryable<Groupe> groupesQuery = _context.Groupes;

            if (chargeAffairesId.HasValue)
            {
                groupesQuery = groupesQuery.Where(g => _context.ObjectifsCreditDepots
                    .Any(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025 && o.EmployeId == chargeAffairesId.Value));
            }

            return groupesQuery.Select(g => new ObjectifDepotViewModel
            {
                Groupe = g,
                GroupeId = g.Id,
                GroupeLibelle = g.Nom,
                Encours2023_TndDat = (decimal?)_context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAT" &&
                                e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                    .Sum(e => e.Solde) ?? 0m,

                Encours2023_TndDav = (decimal?)_context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAV" &&
                                e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                    .Sum(e => e.Solde) ?? 0m,

                Encours2024_TndDat = (decimal?)_context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAT" &&
                                e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                    .Sum(e => e.Solde) ?? 0m,

                Encours2024_TndDav = (decimal?)_context.Encours
                    .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAV" &&
                                e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                    .Sum(e => e.Solde) ?? 0m,

                Objectif2025 = _context.ObjectifsCreditDepots
                    .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025)
                    .Select(o => new Objectif2025ViewModel
                    {
                        MontantDat = o.MontantDat,
                        MontantDav = o.MontantDav
                    })
                    .FirstOrDefault() ?? new Objectif2025ViewModel(),
                EmployeeId = _context.ObjectifsCreditDepots
                    .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025)
                    .Select(o => (int?)o.EmployeId)
                    .FirstOrDefault()
            }).ToList();
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
                        var objectif = _context.ObjectifsCreditDepots.FirstOrDefault(o => o.GroupeId == item.GroupeId && o.TypeObjectif == "Dépôt" && o.Annee == 2025);

                        decimal montantTotal = (item.Objectif2025?.MontantDat ?? 0m) + (item.Objectif2025?.MontantDav ?? 0m);

                        if (objectif != null)
                        {
                            objectif.Montant = montantTotal;
                            objectif.EmployeId = item.EmployeeId;
                            objectif.MontantDat = item.Objectif2025?.MontantDat;
                            objectif.MontantDav = item.Objectif2025?.MontantDav;
                            _context.Entry(objectif).State = EntityState.Modified;
                        }
                        else if (montantTotal > 0)
                        {
                            objectif = new ObjectifCreditDepot
                            {
                                GroupeId = item.GroupeId,
                                TypeObjectif = "Dépôt",
                                Montant = montantTotal,
                                MontantDat = item.Objectif2025?.MontantDat,
                                MontantDav = item.Objectif2025?.MontantDav,
                                DateDebut = new DateTime(2025, 1, 1),
                                DateFin = new DateTime(2025, 12, 31),
                                EmployeId = item.EmployeeId
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
                    return RedirectToAction("Dashboard", "Objectivations");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving changes: {ex.Message}");
                    TempData["Message"] = "Erreur lors de l'enregistrement : " + ex.Message;
                    TempData["IsSuccess"] = false;
                }
            }

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
            try
            {
                var creditData = _context.CreditObjectifs.ToList();
                ViewBag.TotalCreditObjectives = creditData.Count();
                ViewBag.TotalCreditValue = creditData.Sum(o => o.MontantObjectif);
                ViewBag.AverageCreditValue = creditData.Any() ? creditData.Average(o => o.MontantObjectif) : 0;
                ViewBag.CreditData = creditData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreditDashboard: {ex.Message}");
                ViewBag.TotalCreditObjectives = 0;
                ViewBag.TotalCreditValue = 0;
                ViewBag.AverageCreditValue = 0;
                ViewBag.CreditData = new List<CreditObjectif>();
            }
            return View("CreditDashboard");
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
            return View("DepotDashboard");
        }

        // Action to fetch groups data based on selected employee (for AJAX)
        [HttpGet]
        public async Task<IActionResult> GetGroupesData(int? employeId)
        {
            if (!employeId.HasValue)
            {
                return Json(new List<ObjectifDepotViewModel>());
            }

            var groupesAvecEncours = await _context.Groupes
                .Where(g => _context.ObjectifsCreditDepots
                    .Any(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025 && o.EmployeId == employeId.Value))
                .Select(g => new ObjectifDepotViewModel
                {
                    Groupe = g,
                    GroupeId = g.Id,
                    GroupeLibelle = g.Nom,
                    Encours2023_TndDat = (decimal?)_context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAT" &&
                                    e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                        .Sum(e => e.Solde) ?? 0m,

                    Encours2023_TndDav = (decimal?)_context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAV" &&
                                    e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2023)
                        .Sum(e => e.Solde) ?? 0m,

                    Encours2024_TndDat = (decimal?)_context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAT" &&
                                    e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                        .Sum(e => e.Solde) ?? 0m,

                    Encours2024_TndDav = (decimal?)_context.Encours
                        .Where(e => e.GroupeId == g.Id && e.TypeEncours == "Dépôt DAV" &&
                                    e.DateDerniereTransaction.HasValue && e.DateDerniereTransaction.Value.Year == 2024)
                        .Sum(e => e.Solde) ?? 0m,

                    Objectif2025 = _context.ObjectifsCreditDepots
                        .Where(o => o.GroupeId == g.Id && o.TypeObjectif == "Dépôt" && o.Annee == 2025)
                        .Select(o => new Objectif2025ViewModel
                        {
                            MontantDat = o.MontantDat,
                            MontantDav = o.MontantDav
                        })
                        .FirstOrDefault() ?? new Objectif2025ViewModel()
                }).ToListAsync();

            return Json(groupesAvecEncours);
        }
    }
}