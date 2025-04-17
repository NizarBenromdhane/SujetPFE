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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ObjectifCreditDepot objectifCreditDepot)
        {
            if (ModelState.IsValid)
            {
                _context.ObjectifsCreditDepots.Add(objectifCreditDepot);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(objectifCreditDepot);
        }

        public async Task<IActionResult> Index()
        {
            var objectifs = await _context.ObjectifsCreditDepots.ToListAsync();
            return View(objectifs);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifCreditDepot = await _context.ObjectifsCreditDepots.FindAsync(id);
            if (objectifCreditDepot == null)
            {
                return NotFound();
            }
            return View(objectifCreditDepot);
        }

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var objectifCreditDepot = await _context.ObjectifsCreditDepots.FindAsync(id);
            if (objectifCreditDepot == null)
            {
                return NotFound();
            }

            return View(objectifCreditDepot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var objectifCreditDepot = await _context.ObjectifsCreditDepots.FindAsync(id);
            _context.ObjectifsCreditDepots.Remove(objectifCreditDepot);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ObjectifCreditDepotExists(int id)
        {
            return _context.ObjectifsCreditDepots.Any(e => e.Id == id);
        }
    }
}