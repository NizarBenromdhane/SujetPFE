using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;

namespace SujetPFE.Controllers
{
    public class ObjectifSuivisController : Controller
    {
        private readonly PcbContext _context;

        public ObjectifSuivisController(PcbContext context)
        {
            _context = context;
        }

        // GET: ObjectifSuivis (Tableau de bord)
        public IActionResult Index()
        {
            // Récupérer les données nécessaires pour le tableau de bord
            var objectifsSuivis = _context.ObjectifsSuivis
                .Include(os => os.Objective)
                .ThenInclude(o => o.Employee) // Inclure l'employé associé à l'objectif
                .ToList();

            // Calculer les statistiques ou les données agrégées nécessaires
            var totalRealisations = objectifsSuivis.Sum(os => os.Realisation);

            // Vérifier si la liste est vide avant de calculer l'écart moyen
            var ecartMoyen = objectifsSuivis.Any() ? objectifsSuivis.Average(os => os.Ecart) : 0; // Utiliser 0 comme valeur par défaut si la liste est vide

            // Transmettre les données à la vue
            ViewBag.TotalRealisations = totalRealisations;
            ViewBag.EcartMoyen = ecartMoyen;
            return View("Dashboard", objectifsSuivis); // Renvoyer la vue Dashboard.cshtml
        }

        // GET: ObjectifSuivis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifSuivi = await _context.ObjectifsSuivis
                .Include(os => os.Objective)
                .ThenInclude(o => o.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (objectifSuivi == null)
            {
                return NotFound();
            }

            return View(objectifSuivi);
        }

        // GET: ObjectifSuivis/Create
        public IActionResult Create()
        {
            ViewData["ObjectifId"] = new SelectList(_context.Objectives, "Id", "Description"); // Charger les objectifs pour la liste déroulante
            return View();
        }

        // POST: ObjectifSuivis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ObjectifId,Realisation,Ecart,DateSuivi")] ObjectifSuivi objectifSuivi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(objectifSuivi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ObjectifId"] = new SelectList(_context.Objectives, "Id", "Description", objectifSuivi.ObjectifId); // Recharger les objectifs en cas d'erreur
            return View(objectifSuivi);
        }

        // GET: ObjectifSuivis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifSuivi = await _context.ObjectifsSuivis.FindAsync(id);
            if (objectifSuivi == null)
            {
                return NotFound();
            }

            ViewData["ObjectifId"] = new SelectList(_context.Objectives, "Id", "Description", objectifSuivi.ObjectifId); // Charger les objectifs pour la liste déroulante
            return View(objectifSuivi);
        }

        // POST: ObjectifSuivis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ObjectifId,Realisation,Ecart,DateSuivi")] ObjectifSuivi objectifSuivi)
        {
            if (id != objectifSuivi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(objectifSuivi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectifSuiviExists(objectifSuivi.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ObjectifId"] = new SelectList(_context.Objectives, "Id", "Description", objectifSuivi.ObjectifId); // Recharger les objectifs en cas d'erreur
            return View(objectifSuivi);
        }

        // GET: ObjectifSuivis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifSuivi = await _context.ObjectifsSuivis
                .Include(os => os.Objective)
                .ThenInclude(o => o.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (objectifSuivi == null)
            {
                return NotFound();
            }

            return View(objectifSuivi);
        }

        // POST: ObjectifSuivis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objectifSuivi = await _context.ObjectifsSuivis.FindAsync(id);
            if (objectifSuivi != null)
            {
                _context.ObjectifsSuivis.Remove(objectifSuivi);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObjectifSuiviExists(int id)
        {
            return _context.ObjectifsSuivis.Any(e => e.Id == id);
        }
    }
}