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
    public class PratiquesManagérialesController : Controller
    {
        private readonly PcbContext _context;

        public PratiquesManagérialesController(PcbContext context)
        {
            _context = context;
        }

        // GET: PratiquesManagériales
        public async Task<IActionResult> Index()
        {
            return View(await _context.PratiquesManagériales.ToListAsync());
        }

        // GET: PratiquesManagériales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratiquesManagériales = await _context.PratiquesManagériales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pratiquesManagériales == null)
            {
                return NotFound();
            }

            return View(pratiquesManagériales);
        }

        // GET: PratiquesManagériales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PratiquesManagériales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Email,Telephone")] PratiquesManagériales pratiquesManagériales)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pratiquesManagériales);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pratiquesManagériales);
        }

        // GET: PratiquesManagériales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratiquesManagériales = await _context.PratiquesManagériales.FindAsync(id);
            if (pratiquesManagériales == null)
            {
                return NotFound();
            }
            return View(pratiquesManagériales);
        }

        // POST: PratiquesManagériales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Email,Telephone")] PratiquesManagériales pratiquesManagériales)
        {
            if (id != pratiquesManagériales.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pratiquesManagériales);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PratiquesManagérialesExists(pratiquesManagériales.Id))
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
            return View(pratiquesManagériales);
        }

        // GET: PratiquesManagériales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pratiquesManagériales = await _context.PratiquesManagériales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pratiquesManagériales == null)
            {
                return NotFound();
            }

            return View(pratiquesManagériales);
        }

        // POST: PratiquesManagériales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pratiquesManagériales = await _context.PratiquesManagériales.FindAsync(id);
            if (pratiquesManagériales != null)
            {
                _context.PratiquesManagériales.Remove(pratiquesManagériales);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PratiquesManagérialesExists(int id)
        {
            return _context.PratiquesManagériales.Any(e => e.Id == id);
        }
    }
}
