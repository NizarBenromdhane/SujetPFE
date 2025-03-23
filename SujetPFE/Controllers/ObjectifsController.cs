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
    public class ObjectifsController : Controller
    {
        private readonly PcbContext _context;

        public ObjectifsController(PcbContext context)
        {
            _context = context;
        }

        // GET: Objectifs
        public async Task<IActionResult> Index()
        {
            var pcbContext = _context.Objectives.Include(o => o.Client);
            return View(await pcbContext.ToListAsync());
        }

        // GET: Objectifs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectif = await _context.Objectives
                .Include(o => o.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (objectif == null)
            {
                return NotFound();
            }

            return View(objectif);
        }

        // GET: Objectifs/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Activite");
            return View();
        }

        // POST: Objectifs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DateDebut,DateFin,ValeurCible,ValeurActuelle,UniteMesure,Statut,Matricule1,ClientId,MontantCredit,MontantDepot,TypeProduit")] Objectif objectif)
        {
            if (ModelState.IsValid)
            {
                _context.Add(objectif);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Activite", objectif.ClientId);
            return View(objectif);
        }

        // GET: Objectifs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectif = await _context.Objectives.FindAsync(id);
            if (objectif == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Activite", objectif.ClientId);
            return View(objectif);
        }

        // POST: Objectifs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DateDebut,DateFin,ValeurCible,ValeurActuelle,UniteMesure,Statut,Matricule1,ClientId,MontantCredit,MontantDepot,TypeProduit")] Objectif objectif)
        {
            if (id != objectif.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(objectif);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectifExists(objectif.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Activite", objectif.ClientId);
            return View(objectif);
        }

        // GET: Objectifs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectif = await _context.Objectives
                .Include(o => o.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (objectif == null)
            {
                return NotFound();
            }

            return View(objectif);
        }

        // POST: Objectifs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objectif = await _context.Objectives.FindAsync(id);
            if (objectif != null)
            {
                _context.Objectives.Remove(objectif);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObjectifExists(int id)
        {
            return _context.Objectives.Any(e => e.Id == id);
        }
    }
}
