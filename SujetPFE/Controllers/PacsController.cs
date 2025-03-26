using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Infrastructure;
using SujetPFE.Models;
using Newtonsoft.Json; // N'oubliez pas d'ajouter cette directive using

namespace SujetPFE.Controllers
{
    public class PacsController : Controller
    {
        private readonly PcbContext _context;

        public PacsController(PcbContext context)
        {
            _context = context;
        }

        // GET: Pacs
        public async Task<IActionResult> Index()
        {
            // Eager loading des KPIs (si nécessaire)
            var pacsWithKpis = await _context.Pacs
                .Include(p => p.KPIValues) // Assurez-vous que le nom de votre propriété est correct (KPIValues)
                .ToListAsync();
            return View(pacsWithKpis);
        }

        // GET: Pacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pac = await _context.Pacs
                .Include(p => p.KPIValues) // Charger les KPIs pour la page de détails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pac == null)
            {
                return NotFound();
            }

            return View(pac);
        }

        // GET: Pacs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pac pac, string kpiData) // Récupérer les données des KPIs
        {
            if (ModelState.IsValid)
            {
                // Deserialiser les données des KPIs
                if (!string.IsNullOrEmpty(kpiData))
                {
                    try
                    {
                        var kpiValues = JsonConvert.DeserializeObject<List<KPIValue>>(kpiData);
                        pac.KPIValues = kpiValues; // Assigner les KPIs au PAC
                    }
                    catch (JsonException ex)
                    {
                        ModelState.AddModelError("kpiData", "Invalid KPI data format.");
                        return View(pac); // Retourner à la vue avec l'erreur
                    }
                }

                _context.Add(pac);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pac);
        }

        // GET: Pacs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pac = await _context.Pacs
                .Include(p => p.KPIValues) // Charger les KPIs pour l'édition
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pac == null)
            {
                return NotFound();
            }
            return View(pac);
        }

        // POST: Pacs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pac pac, string kpiData) // Récupérer les données des KPIs
        {
            if (id != pac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Deserialiser les données des KPIs
                    if (!string.IsNullOrEmpty(kpiData))
                    {
                        try
                        {
                            var kpiValues = JsonConvert.DeserializeObject<List<KPIValue>>(kpiData);
                            // Mettre à jour les KPIs existants
                            var existingKpis = await _context.KPIValues.Where(k => k.PacId == pac.Id).ToListAsync();
                            _context.KPIValues.RemoveRange(existingKpis); // Supprimer les anciens KPIs
                            pac.KPIValues = kpiValues; // Ajouter les nouveaux KPIs
                        }
                        catch (JsonException ex)
                        {
                            ModelState.AddModelError("kpiData", "Invalid KPI data format.");
                            return View(pac); // Retourner à la vue avec l'erreur
                        }
                    }
                    else
                    {
                        var existingKpis = await _context.KPIValues.Where(k => k.PacId == pac.Id).ToListAsync();
                        _context.KPIValues.RemoveRange(existingKpis);
                    }
                    _context.Update(pac);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacExists(pac.Id))
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
            return View(pac);
        }

        // GET: Pacs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pac = await _context.Pacs
                .Include(p => p.KPIValues) // Charger les KPIs pour la confirmation de suppression
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pac == null)
            {
                return NotFound();
            }

            return View(pac);
        }

        // POST: Pacs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pac = await _context.Pacs.FindAsync(id);
            if (pac == null)
            {
                return NotFound(); // Handle the case where the entity to delete doesn't exist
            }
            // Supprimer les KPIs associés
            var kpisToDelete = await _context.KPIValues.Where(k => k.PacId == id).ToListAsync();
            _context.KPIValues.RemoveRange(kpisToDelete);
            _context.Pacs.Remove(pac);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacExists(int id)
        {
            return _context.Pacs.Any(e => e.Id == id);
        }
    }
}
