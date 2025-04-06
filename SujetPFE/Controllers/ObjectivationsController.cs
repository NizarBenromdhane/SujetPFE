using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure; // Remplacez par le namespace de votre contexte
using System.Linq;

namespace SujetPFE.Controllers
{
    public class ObjectivationsController : Controller
    {
        private readonly PcbContext _context;

        public ObjectivationsController(PcbContext context)
        {
            _context = context;
        }

        public IActionResult SaisieDepot()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SaisieCredit()
        {
            // Récupérer la liste des employés pour la liste déroulante
            var employes = _context.Employees
                                    .Select(e => new SelectListItem { Value = e.Id.ToString(), Text = e.Nom })
                                    .ToList();
            ViewBag.Employes = employes;

            // Récupérer la liste des groupes pour la liste déroulante
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
                _context.CreditObjectifs.Add(model); // Assurez-vous d'avoir DbSet<CreditObjectif> dans votre contexte
                _context.SaveChanges();

                ViewBag.Message = "L'objectif de crédit a été enregistré avec succès.";
                ViewBag.IsSuccess = true;
                ModelState.Clear(); // Vider le formulaire après succès
                return View();
            }

            // En cas d'erreur de validation, repopuler les listes déroulantes
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

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}