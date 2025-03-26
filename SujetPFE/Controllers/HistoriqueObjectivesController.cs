using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Infrastructure;

namespace SujetPFE.Controllers
{
    public class HistoriqueObjectivesController : Controller
    {
        private readonly PcbContext _context;

        public HistoriqueObjectivesController(PcbContext context)
        {
            _context = context;
        }

        // GET: HistoriqueObjectifs
        public async Task<IActionResult> Index()
        {
            return View(await _context.HistoriqueObjectifs.ToListAsync());
        }

        // GET: HistoriqueObjectifs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historiqueObjectif = await _context.HistoriqueObjectifs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historiqueObjectif == null)
            {
                return NotFound();
            }

            return View(historiqueObjectif);
        }

        // GET: HistoriqueObjectifs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistoriqueObjectifs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Date,ObjectifId")] HistoriqueObjectif historiqueObjectif)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historiqueObjectif);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historiqueObjectif);
        }

        // GET: HistoriqueObjectifs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historiqueObjectif = await _context.HistoriqueObjectifs.FindAsync(id);
            if (historiqueObjectif == null)
            {
                return NotFound();
            }
            return View(historiqueObjectif);
        }

        // POST: HistoriqueObjectifs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Date,ObjectifId")] HistoriqueObjectif historiqueObjectif)
        {
            if (id != historiqueObjectif.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historiqueObjectif);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoriqueObjectifExists(historiqueObjectif.Id))
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
            return View(historiqueObjectif);
        }

        // GET: HistoriqueObjectifs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historiqueObjectif = await _context.HistoriqueObjectifs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (historiqueObjectif == null)
            {
                return NotFound();
            }

            return View(historiqueObjectif);
        }

        // POST: HistoriqueObjectifs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historiqueObjectif = await _context.HistoriqueObjectifs.FindAsync(id);
            if (historiqueObjectif != null)
            {
                _context.HistoriqueObjectifs.Remove(historiqueObjectif);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoriqueObjectifExists(int id)
        {
            return _context.HistoriqueObjectifs.Any(e => e.Id == id);
        }
    }
}
