using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SujetPFE.Infrastructure; // Replace with your actual namespace
using SujetPFE.Services; // Ensure this namespace is correct
using Microsoft.Extensions.Configuration;
using System.Linq;
using SujetPFE.Domain.Entities; // Make sure your Groupe entity is here
using System.Collections.Generic; // Ensure this using statement is present

namespace SujetPFE.Controllers
{
    public class ObjectivationsController : Controller
    {
        private readonly PcbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ExcelServicesemployee _excelServicesEmployee; // Inject ExcelServicesemployee
        private readonly ExcelToGroupeMapper _excelToGroupeMapper; // Inject ExcelToGroupeMapper

        public ObjectivationsController(PcbContext context, IConfiguration configuration, ExcelServicesemployee excelServicesEmployee, ExcelToGroupeMapper excelToGroupeMapper)
        {
            _context = context;
            _configuration = configuration;
            _excelServicesEmployee = excelServicesEmployee;
            _excelToGroupeMapper = excelToGroupeMapper;
        }

        [HttpGet]
        public IActionResult SaisieDepot()
        {
            // Récupérer le chemin du fichier Excel des employés depuis la configuration
            string employesExcelPath = _configuration["CheminFichierEmployesExcel"];

            if (string.IsNullOrEmpty(employesExcelPath) || !System.IO.File.Exists(employesExcelPath))
            {
                ViewBag.ErrorMessage = "Le fichier des employés Excel n'est pas trouvé.";
                return View();
            }

            // Utiliser ExcelServicesemployee pour lire les employés
            List<SelectListItem> employesFromExcel = _excelServicesEmployee.GetEmployesFromExcel(employesExcelPath);
            ViewBag.Employes = employesFromExcel;

            // Récupérer le chemin du fichier Excel des groupes depuis la configuration
            string groupesExcelPath = _configuration["CheminFichierGroupesExcel"];

            if (!string.IsNullOrEmpty(groupesExcelPath) && System.IO.File.Exists(groupesExcelPath))
            {
                // Utiliser ExcelToGroupeMapper pour lire les groupes
                var groupesFromExcel = _excelToGroupeMapper.MapGroupesFromExcel(groupesExcelPath);
                ViewBag.Groupes = groupesFromExcel;
            }
            else
            {
                ViewBag.GroupesErrorMessage = "Le fichier des groupes Excel n'est pas trouvé.";
                ViewBag.Groupes = new List<Groupe>(); // Initialize an empty list to avoid null errors in the view
            }

            return View();
        }

        [HttpGet]
        public IActionResult SaisieCredit()
        {
            // Récupérer la liste des employés depuis la base de données (inchangé)
            var employes = _context.Employees
                                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                                    .ToList();
            ViewBag.Employes = employes;

            // Récupérer la liste des groupes depuis la base de données (inchangé)
            var groupes = _context.Groupes
                                   .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Nom })
                                   .ToList();
            ViewBag.Groupes = groupes;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaisieCredit(CreditObjectif model)
        {
            if (ModelState.IsValid)
            {
                _context.CreditObjectifs.Add(model);
                _context.SaveChanges();

                ViewBag.Message = "L'objectif de crédit a été enregistré avec succès.";
                ViewBag.IsSuccess = true;
                ModelState.Clear();
                return View();
            }

            // Repopuler les listes déroulantes en cas d'erreur (employés depuis la base de données)
            var employes = _context.Employees
                                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                                    .ToList();
            ViewBag.Employes = employes;

            var groupes = _context.Groupes
                                   .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Nom })
                                   .ToList();
            ViewBag.Groupes = groupes;

            ViewBag.Message = "Erreur lors de l'enregistrement de l'objectif de crédit.";
            ViewBag.IsSuccess = false;
            return View(model);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreditDashboard()
        {
            var creditData = _context.CreditObjectifs.ToList();

            // Calculate basic statistics
            ViewBag.TotalCreditObjectives = creditData.Count();
            ViewBag.TotalCreditValue = creditData.Sum(o => o.MontantObjectif);
            ViewBag.AverageCreditValue = creditData.Any() ? creditData.Average(o => o.MontantObjectif) : 0;

            ViewBag.CreditData = creditData;
            return View("CreditDashboard");
        }

        [HttpGet]
        public IActionResult DepotDashboard()
        {
            // Récupérer les objectifs de dépôt et s'assurer que la liste n'est pas null
            var depotData = _context.ObjectifsCreditDepot.Where(o => o.TypeObjectif == "Dépôt").ToList() as List<ObjectifCreditDepot> ?? new List<ObjectifCreditDepot>();

            // Calculate basic statistics for depots
            ViewBag.TotalDepotObjectives = depotData.Count();
            ViewBag.TotalDepotValue = depotData.Sum(o => o.Montant); // Use the 'Montant' property
            ViewBag.AverageDepotValue = depotData.Any() ? depotData.Average(o => o.Montant) : 0; // Use the 'Montant' property

            ViewBag.DepotData = depotData;
            return View("DepotDashboard");
        }
    }
}
