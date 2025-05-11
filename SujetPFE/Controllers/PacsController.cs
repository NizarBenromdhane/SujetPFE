using System;
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

        // GET: Pacs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pac = await _context.Pacs.FirstOrDefaultAsync(m => m.Id == id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pac pac)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pac);
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "PAC créé avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Erreur lors de l'enregistrement du PAC dans la base de données : {ex.InnerException?.Message}");
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

            var pac = await _context.Pacs.FirstOrDefaultAsync(m => m.Id == id);

            if (pac == null)
            {
                return NotFound();
            }
            return View(pac);
        }

        // POST: Pacs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pac pac)
        {
            if (id != pac.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

            var pac = await _context.Pacs.FirstOrDefaultAsync(m => m.Id == id);

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