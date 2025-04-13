using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Infrastructure;
using SujetPFE.Models;

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
            return View(await _context.Pacs.ToListAsync());
        }

        // GET: Pacs/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pac pac, string kpiData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pac);

                List<KPIValue> kpiValues = null;

                if (!string.IsNullOrEmpty(kpiData))
                {
                    try
                    {
                        kpiValues = JsonSerializer.Deserialize<List<KPIValue>>(kpiData);
                        if (kpiValues != null)
                        {
                            foreach (var kpiValue in kpiValues)
                            {
                                kpiValue.PacId = pac.Id;
                                _context.KPIValues.Add(kpiValue);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("kpiData", "Format de données KPI invalide (données JSON vides ou incorrectes).");
                            return View(pac);
                        }
                    }
                    catch (JsonException ex)
                    {
                        ModelState.AddModelError("kpiData", "Format de données KPI invalide.");
                        return View(pac);
                    }
                }
                else
                {
                    pac.KPIValues = new List<KPIValue>(); // Initialize even if no kpiData
                }

                // Server-side validation for at least one KPI (required)
                if (pac.KPIValues == null || pac.KPIValues.Count == 0)
                {
                    ModelState.AddModelError("KPIValues", "Au moins une valeur KPI est requise.");
                    return View(pac);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "PAC créé avec succès."; // Ajouter un message de succès
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Log l'erreur (important pour le débogage)
                    ModelState.AddModelError("", "Erreur lors de l'enregistrement du PAC dans la base de données.");
                    return View(pac);
                }
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
                .Include(p => p.KPIValues)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pac == null)
            {
                return NotFound();
            }

            return View(pac);
        }

        // POST: Pacs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pac pac, string kpiData)
        {
            if (id != pac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(kpiData))
                    {
                        try
                        {
                            var kpiValues = JsonSerializer.Deserialize<List<KPIValue>>(kpiData);
                            if (kpiValues != null)
                            {
                                var existingKpis = await _context.KPIValues.Where(k => k.PacId == pac.Id).ToListAsync();
                                _context.KPIValues.RemoveRange(existingKpis);
                                foreach (var kpiValue in kpiValues)
                                {
                                    kpiValue.PacId = pac.Id;
                                }
                                pac.KPIValues = kpiValues;
                            }
                            else
                            {
                                ModelState.AddModelError("kpiData", "Format de données KPI invalide (données JSON vides ou incorrectes).");
                                return View(pac);
                            }
                        }
                        catch (JsonException ex)
                        {
                            ModelState.AddModelError("kpiData", "Format de données KPI invalide.");
                            return View(pac);
                        }
                    }
                    else
                    {
                        var existingKpis = await _context.KPIValues.Where(k => k.PacId == pac.Id).ToListAsync();
                        _context.KPIValues.RemoveRange(existingKpis);
                        pac.KPIValues = new List<KPIValue>(); // Ensure KPIValues is not null
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
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Erreur lors de la mise à jour du PAC.");
                    return View(pac);
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
                .Include(p => p.KPIValues)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pac == null)
            {
                return NotFound();
            }

            return View(pac);
        }

        // POST: Pacs/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pac = await _context.Pacs.FindAsync(id);
            if (pac == null)
            {
                return NotFound();
            }

            var kpisToDelete = await _context.KPIValues.Where(k => k.PacId == id).ToListAsync();
            _context.KPIValues.RemoveRange(kpisToDelete);

            _context.Pacs.Remove(pac);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Erreur lors de la suppression du PAC.");
                return View(pac);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PacExists(int id)
        {
            return _context.Pacs.Any(e => e.Id == id);
        }
    }
}