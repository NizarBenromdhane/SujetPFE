using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;
using System.Collections.Generic;

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
            // ** Option 1: Afficher uniquement les données de la base de données **
            var suivisIRC = await _context.SuivisIRC
                .Include(s => s.Client)
                .Include(s => s.RDVs)
                .ToListAsync();
            return View(suivisIRC);

            // ** Option 2: Afficher les données de test (avec IDs corrigés) **
            // var suivisIRCTest = new List<SuiviIRC>()
            // {
            //     new SuiviIRC {
            //         Id = 1001,
            //         ChargeAffaires = "Test CA 1",
            //         ClientId = 1,
            //         Commentaires = "Commentaire Test 1",
            //         NombreVisites = 2,
            //         NombreRDV = 1,
            //         DateVisite = DateTime.Now.AddDays(-5),
            //         Client = new Client { Id = 1, RaisonSociale = "Client Test A" },
            //         RDVs = new List<RDV>() { new RDV { Id = 2001, Affaire = "RDV Test A1", SuiviIRCId = 1001, Date = DateTime.Now.AddDays(-6) } }
            //     },
            //     new SuiviIRC {
            //         Id = 1002,
            //         ChargeAffaires = "Test CA 2",
            //         ClientId = 2,
            //         Commentaires = "Commentaire Test 2",
            //         NombreVisites = 5,
            //         NombreRDV = 3,
            //         DateVisite = DateTime.Now.AddDays(-10),
            //         Client = new Client { Id = 2, RaisonSociale = "Client Test B" },
            //         RDVs = new List<RDV>() {
            //             new RDV { Id = 2002, Affaire = "RDV Test B1", SuiviIRCId = 1002, Date = DateTime.Now.AddDays(-11) },
            //             new RDV { Id = 2003, Affaire = "RDV Test B2", SuiviIRCId = 1002, Date = DateTime.Now.AddDays(-12) }
            //         }
            //     },
            //     new SuiviIRC {
            //         Id = 1003,
            //         ChargeAffaires = "Test CA 3",
            //         ClientId = 1,
            //         Commentaires = "Commentaire Test 3",
            //         NombreVisites = 1,
            //         NombreRDV = 1,
            //         DateVisite = DateTime.Now.AddDays(-2),
            //         Client = new Client { Id = 1, RaisonSociale = "Client Test C" },
            //         RDVs = new List<RDV>() { new RDV { Id = 2004, Affaire = "RDV Test C1", SuiviIRCId = 1003, Date = DateTime.Now.AddDays(-3) } }
            //     }
            // };
            // return View(suivisIRCTest);

            // ** Option 3: Combiner les données de test AVEC les données de la base de données **
            // var suivisIRCFromDb = await _context.SuivisIRC
            //     .Include(s => s.Client)
            //     .Include(s => s.RDVs)
            //     .ToListAsync();
            // var combinedList = suivisIRCTest.Concat(suivisIRCFromDb).ToList();
            // return View(combinedList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var suiviIRC = await _context.SuivisIRC
                .Include(s => s.Client)
                .Include(s => s.RDVs)
                    .ThenInclude(r => r.CompteRendu)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (suiviIRC == null) return NotFound();

            return View(suiviIRC);
        }

        // GET: SuiviIRC/Create
        public IActionResult Create()
        {
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom");
            return View();
        }

        // POST: SuiviIRC/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,DateVisite,ChargeAffaires,NombreVisites,NombreRDV,Commentaires")] SuiviIRC suiviIRC)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suiviIRC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", suiviIRC.ClientId);
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
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", suiviIRC.ClientId);
            return View(suiviIRC);
        }

        // POST: SuiviIRC/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,DateVisite,ChargeAffaires,NombreVisites,NombreRDV,MoyenneRDV,Commentaires")] SuiviIRC suiviIRC)
        {
            if (id != suiviIRC.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suiviIRC);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", suiviIRC.ClientId);
            return View(suiviIRC);
        }

        // GET: SuiviIRC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suiviIRC = await _context.SuivisIRC
                .Include(s => s.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
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

        [HttpGet]
        public async Task<IActionResult> AjouterRDV(int suiviIRCId)
        {
            ViewBag.SuiviIRCId = suiviIRCId;
            ViewBag.Clients = new SelectList(await _context.Clients.ToListAsync(), "Id", "Nom");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterRDV(RDV rdv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rdv);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Le RDV a été ajouté avec succès.";
                return RedirectToAction("Index", "SuiviIRC");
            }

            ViewBag.SuiviIRCId = rdv.SuiviIRCId;
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Nom", rdv.ClientId);
            return View(rdv);
        }

        // GET: SuiviIRC/EditRDV/5
        public async Task<IActionResult> EditRDV(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rdv = await _context.RDVs
                .Include(r => r.Client) // Inclure le client pour le dropdown
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rdv == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", rdv.ClientId);
            return View(rdv);
        }

        // POST: SuiviIRC/EditRDV/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRDV(int id, [Bind("Id,Date,ClientId,Commentaires,NombreVisites,Lieu,Affaire,SuiviIRCId")] RDV rdv)
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
                    return RedirectToAction(nameof(Index)); // Rediriger vers la liste après la modification
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
            ViewBag.ClientId = new SelectList(_context.Clients, "Id", "Nom", rdv.ClientId);
            return View(rdv);
        }

        // ... [Reste des actions CompteRendu et méthodes helpers] ...

        private async Task UpdateSuiviIRCStats(int suiviIRCId)
        {
            var suiviIRC = await _context.SuivisIRC.FindAsync(suiviIRCId);
            if (suiviIRC != null)
            {
                suiviIRC.NombreRDV = await _context.RDVs
                    .CountAsync(r => r.SuiviIRCId == suiviIRCId);

                suiviIRC.MoyenneRDV = suiviIRC.NombreVisites > 0
                    ? (double)suiviIRC.NombreRDV / suiviIRC.NombreVisites
                    : 0;

                _context.Update(suiviIRC);
                await _context.SaveChangesAsync();
            }
        }

        private bool SuiviIRCExists(int id) => _context.SuivisIRC.Any(e => e.Id == id);
        private bool RDVExists(int id) => _context.RDVs.Any(e => e.Id == id);
        private bool CompteRenduExists(int id) => _context.ComptesRendus.Any(e => e.Id == id);
    }
}