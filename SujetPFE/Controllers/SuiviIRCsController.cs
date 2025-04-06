using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Infrastructure;
using SujetPFE.Domain.Entities;

namespace SujetPFE.Controllers
{
    public class SuiviIRCController : Controller
    {
        private readonly PcbContext _context;

        public SuiviIRCController(PcbContext context)
        {
            _context = context;
        }

        // GET: SuiviIRC
        public async Task<IActionResult> Index()
        {
            var suivisIRC = await _context.SuivisIRC.Include(s => s.Client).ToListAsync();
            return View(suivisIRC);
        }

        // GET: SuiviIRC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suiviIRC = await _context.SuivisIRC.Include(s => s.Client).FirstOrDefaultAsync(m => m.Id == id);
            if (suiviIRC == null)
            {
                return NotFound();
            }

            return View(suiviIRC);
        }

        // GET: SuiviIRC/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale");
            return View();
        }

        // POST: SuiviIRC/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,DateVisite,ChargeAffaires,NombreVisites,NombreRDV,Commentaires")] SuiviIRC suiviIRC)
        {
            if (ModelState.IsValid)
            {
                if (suiviIRC.NombreRDV > 0 && suiviIRC.NombreVisites > 0)
                {
                    suiviIRC.MoyenneRDV = (double)suiviIRC.NombreRDV / suiviIRC.NombreVisites;
                }
                else
                {
                    suiviIRC.MoyenneRDV = 0;
                }

                _context.Add(suiviIRC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = suiviIRC.Id });
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale", suiviIRC.ClientId);
            return View(suiviIRC);
        }

        // GET: SuiviIRC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suiviIRC = await _context.SuivisIRC.FindAsync(id);
            if (suiviIRC == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale", suiviIRC.ClientId);
            return View(suiviIRC);
        }

        // POST: SuiviIRC/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,DateVisite,ChargeAffaires,NombreVisites,NombreRDV,Commentaires")] SuiviIRC suiviIRC)
        {
            if (id != suiviIRC.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (suiviIRC.NombreRDV > 0 && suiviIRC.NombreVisites > 0)
                    {
                        suiviIRC.MoyenneRDV = (double)suiviIRC.NombreRDV / suiviIRC.NombreVisites;
                    }
                    else
                    {
                        suiviIRC.MoyenneRDV = 0;
                    }

                    _context.Update(suiviIRC);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = suiviIRC.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuiviIRCExists(suiviIRC.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale", suiviIRC.ClientId);
            return View(suiviIRC);
        }

        // GET: SuiviIRC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suiviIRC = await _context.SuivisIRC.Include(s => s.Client).FirstOrDefaultAsync(m => m.Id == id);
            if (suiviIRC == null)
            {
                return NotFound();
            }

            return View(suiviIRC);
        }

        // POST: SuiviIRC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suiviIRC = await _context.SuivisIRC.FindAsync(id);
            _context.SuivisIRC.Remove(suiviIRC);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuiviIRCExists(int id)
        {
            return _context.SuivisIRC.Any(e => e.Id == id);
        }

        // GET: SuiviIRC/AjouterRDV
        public IActionResult AjouterRDV()
        {
            return View();
        }

        // POST: SuiviIRC/AjouterRDV
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterRDV([Bind("Id,Date,Client,Commentaires,NombreVisites")] RDV rdv)
        {
            if (ModelState.IsValid)
            {
                _context.RDVs.Add(rdv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rdv);
        }

        // GET: SuiviIRC/ModifierRDV/5
        public async Task<IActionResult> ModifierRDV(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rdv = await _context.RDVs.FindAsync(id);
            if (rdv == null)
            {
                return NotFound();
            }
            return View(rdv);
        }

        // POST: SuiviIRC/ModifierRDV/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifierRDV(int id, [Bind("Id,Date,Client,Commentaires,NombreVisites")] RDV rdv)
        {
            if (id != rdv.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rdv);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RDVExists(rdv.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(rdv);
        }

        // GET: SuiviIRC/CompteRendu/5
        public async Task<IActionResult> CompteRendu(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compteRendu = await _context.ComptesRendus.FirstOrDefaultAsync(cr => cr.RDVId == id);
            if (compteRendu == null)
            {
                return NotFound();
            }
            return View(compteRendu);
        }

        private bool RDVExists(int id)
        {
            return _context.RDVs.Any(e => e.Id == id);
        }
    }
}