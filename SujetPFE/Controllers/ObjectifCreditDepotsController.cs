using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;

namespace SujetPFE.Controllers
{
    public class ObjectifCreditDepotsController : Controller
    {
        private readonly PcbContext _context;

        public ObjectifCreditDepotsController(PcbContext context)
        {
            _context = context;
        }

        // GET: ObjectifCreditDepots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ObjectifCreditDepots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ObjectifCreditDepot objectifCreditDepot)
        {
            if (ModelState.IsValid)
            {
                _context.ObjectifsCreditDepot.Add(objectifCreditDepot);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Redirigez vers l'action Index ou une autre action appropriée
            }
            return View(objectifCreditDepot);
        }

        // GET: ObjectifCreditDepots/Index (pour afficher la liste des objectifs)
        public async Task<IActionResult> Index()
        {
            var objectifs = await _context.ObjectifsCreditDepot.ToListAsync();
            return View(objectifs);
        }

        // GET: ObjectifCreditDepots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifCreditDepot = await _context.ObjectifsCreditDepot.FindAsync(id);
            if (objectifCreditDepot == null)
            {
                return NotFound();
            }
            return View(objectifCreditDepot);
        }

        // POST: ObjectifCreditDepots/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ObjectifCreditDepot objectifCreditDepot)
        {
            if (id != objectifCreditDepot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(objectifCreditDepot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectifCreditDepotExists(objectifCreditDepot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(objectifCreditDepot);
        }

        // GET: ObjectifCreditDepots/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifCreditDepot = await _context.ObjectifsCreditDepot.FindAsync(id);
            if (objectifCreditDepot == null)
            {
                return NotFound();
            }

            return View(objectifCreditDepot);
        }

        // POST: ObjectifCreditDepots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objectifCreditDepot = await _context.ObjectifsCreditDepot.FindAsync(id);
            _context.ObjectifsCreditDepot.Remove(objectifCreditDepot);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ObjectifCreditDepotExists(int id)
        {
            return _context.ObjectifsCreditDepot.Any(e => e.Id == id);
        }
    }
}