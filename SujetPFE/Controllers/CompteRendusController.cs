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
    public class CompteRendusController : Controller
    {
        private readonly PcbContext _context;

        public CompteRendusController(PcbContext context)
        {
            _context = context;
        }

        // GET: CompteRendus
        public async Task<IActionResult> Index()
        {
            // ** Utilisation de données de test pour la liste des comptes rendus **
            var comptesRendusDeTest = new List<CompteRendu>()
            {
                new CompteRendu { Id = 1, RDVId = 1, DateCreation = DateTime.Now.AddDays(-2), Contenu = "Contenu du compte rendu Alpha (TEST)",
                    RDV = new RDV { Id = 1, Affaire = "Proposition Projet Alpha (TEST)" } },
                new CompteRendu { Id = 2, RDVId = 2, DateCreation = DateTime.Now.AddDays(-1), Contenu = "Contenu du compte rendu Beta (TEST)",
                    RDV = new RDV { Id = 2, Affaire = "Suivi Contrat Beta (TEST)" } },
                new CompteRendu { Id = 3, RDVId = 3, DateCreation = DateTime.Now, Contenu = "Contenu du compte rendu Gamma (TEST)",
                    RDV = new RDV { Id = 3, Affaire = "Discussion Partenariat Gamma (TEST)" } }
            };

            // Récupérer le nouveau compte rendu de test depuis TempData (s'il existe)
            if (TempData.ContainsKey("NouveauCompteRenduTest"))
            {
                comptesRendusDeTest.Insert(0, TempData["NouveauCompteRenduTest"] as CompteRendu);
            }

            return View(comptesRendusDeTest);

            // ** Pour utiliser les données de la base de données, décommentez ce code **
            /*
            var pcbContext = _context.ComptesRendus.Include(c => c.RDV);
            return View(await pcbContext.ToListAsync());
            */
        }

        // GET: CompteRendus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compteRendu = await _context.ComptesRendus
                .Include(c => c.RDV)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compteRendu == null)
            {
                return NotFound();
            }

            return View(compteRendu);
        }

        // GET: CompteRendus/Create
        public IActionResult Create()
        {
            // ** Utilisation de données de test pour la liste des RDVs **
            var rdvDataDeTest = new List<object>()
            {
                new { Id = 1, Affaire = "Proposition Projet Alpha (TEST)" },
                new { Id = 2, Affaire = "Suivi Contrat Beta (TEST)" },
                new { Id = 3, Affaire = "Discussion Partenariat Gamma (TEST)" },
                new { Id = 4, Affaire = "Nouveau RDV Delta (TEST)" }
            };

            ViewBag.RDVId = new SelectList(rdvDataDeTest, "Id", "Affaire");
            return View();

            // ** Pour utiliser les données de la base de données, décommentez ce code **
            /*
            var rdvList = _context.RDVs.Select(r => new { Id = r.Id, Affaire = r.Affaire }).ToList();
            ViewBag.RDVId = new SelectList(rdvList, "Id", "Affaire");
            return View();
            */
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RDVId,DateCreation,Contenu")] CompteRendu compteRendu)
        {
            // <--- Premier point d'arrêt ici
            if (ModelState.IsValid)
            {
                // <--- Deuxième point d'arrêt ici
                var nouveauCompteRenduTest = new CompteRendu
                {
                    Id = new Random().Next(100),
                    RDVId = compteRendu.RDVId,
                    DateCreation = compteRendu.DateCreation,
                    Contenu = compteRendu.Contenu,
                    RDV = new RDV
                    {
                        Id = compteRendu.RDVId,
                        Affaire = ((SelectList)ViewBag.RDVId).FirstOrDefault(r => r.Value == compteRendu.RDVId.ToString())?.Text
                    }
                };

                TempData["NouveauCompteRenduTest"] = nouveauCompteRenduTest;

                // <--- Troisième point d'arrêt ici
                return RedirectToAction(nameof(Index));
            }

            // <--- Quatrième point d'arrêt ici (en cas d'erreur de validation)
            var rdvDataDeTest = new List<object>()
            {
                new { Id = 1, Affaire = "Proposition Projet Alpha (TEST)" },
                new { Id = 2, Affaire = "Suivi Contrat Beta (TEST)" },
                new { Id = 3, Affaire = "Discussion Partenariat Gamma (TEST)" },
                new { Id = 4, Affaire = "Nouveau RDV Delta (TEST)" }
            };
            ViewBag.RDVId = new SelectList(rdvDataDeTest, "Id", "Affaire", compteRendu.RDVId);
            return RedirectToAction(nameof(Index));
        }

        // GET: CompteRendus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compteRendu = await _context.ComptesRendus.FindAsync(id);
            if (compteRendu == null)
            {
                return NotFound();
            }
            // Utilisation des données de test pour la liste des RDVs
            var rdvDataDeTest = new List<object>()
            {
                new { Id = 1, Affaire = "Proposition Projet Alpha (TEST)" },
                new { Id = 2, Affaire = "Suivi Contrat Beta (TEST)" },
                new { Id = 3, Affaire = "Discussion Partenariat Gamma (TEST)" },
                new { Id = 4, Affaire = "Nouveau RDV Delta (TEST)" }
            };
            ViewData["RDVId"] = new SelectList(rdvDataDeTest, "Id", "Affaire", compteRendu.RDVId);
            return View(compteRendu);

            // ** Pour utiliser les données de la base de données, décommentez ce code **
            /*
            ViewData["RDVId"] = new SelectList(_context.RDVs, "Id", "Affaire", compteRendu.RDVId);
            return View(compteRendu);
            */
        }

        // POST: CompteRendus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RDVId,DateCreation,Contenu")] CompteRendu compteRendu)
        {
            if (id != compteRendu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // ** Si vous utilisez les données de test pour l'Index, vous ne modifiez pas la base **
                // try
                // {
                //     _context.Update(compteRendu);
                //     await _context.SaveChangesAsync();
                // }
                // catch (DbUpdateConcurrencyException)
                // {
                //     if (!CompteRenduExists(compteRendu.Id))
                //     {
                //         return NotFound();
                //     }
                //     else
                //     {
                //         throw;
                //     }
                // }
                return RedirectToAction(nameof(Index));
            }
            // Utilisation des données de test pour la liste des RDVs
            var rdvDataDeTest = new List<object>()
            {
                new { Id = 1, Affaire = "Proposition Projet Alpha (TEST)" },
                new { Id = 2, Affaire = "Suivi Contrat Beta (TEST)" },
                new { Id = 3, Affaire = "Discussion Partenariat Gamma (TEST)" },
                new { Id = 4, Affaire = "Nouveau RDV Delta (TEST)" }
            };
            ViewData["RDVId"] = new SelectList(rdvDataDeTest, "Id", "Affaire", compteRendu.RDVId);
            return View(compteRendu);

            // ** Pour utiliser les données de la base de données, décommentez ce code **
            /*
            ViewData["RDVId"] = new SelectList(_context.RDVs, "Id", "Affaire", compteRendu.RDVId);
            return View(compteRendu);
            */
        }

        // GET: CompteRendus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // ** Simuler la récupération d'un compte rendu de test **
            var compteRenduTest = (TempData["NouveauCompteRenduTest"] as CompteRendu)?.Id == id ?
                TempData["NouveauCompteRenduTest"] as CompteRendu :
                new CompteRendu { Id = id.Value, RDV = new RDV { Affaire = "RDV Test" }, DateCreation = DateTime.Now, Contenu = "Contenu Test" };

            if (compteRenduTest == null)
            {
                return NotFound();
            }

            return View(compteRenduTest);

            // ** Pour utiliser la base de données, décommentez ce code **
            /*
            var compteRendu = await _context.ComptesRendus
                .Include(c => c.RDV)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compteRendu == null)
            {
                return NotFound();
            }
            return View(compteRendu);
            */
        }

        // POST: CompteRendus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // ** Si vous utilisez les données de test pour l'Index, vous ne supprimez pas de la base **
            // var compteRendu = await _context.ComptesRendus.FindAsync(id);
            // if (compteRendu != null)
            // {
            //     _context.ComptesRendus.Remove(compteRendu);
            // }
            // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompteRenduExists(int id)
        {
            // ** Si vous utilisez les données de test pour l'Index, cette vérification n'est pas pertinente **
            return true;
            //return _context.ComptesRendus.Any(e => e.Id == id);
        }
    }
}