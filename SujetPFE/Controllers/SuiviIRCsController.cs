using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;

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
            return View(await _context.SuivisIRC.Include(s => s.Client).ToListAsync());
        }

        // GET: SuiviIRC/Details/5
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

        // ... [Méthodes Create, Edit, Delete existantes] ...

        // RDV Actions
        public IActionResult AjouterRDV(int suiviIRCId)
        {
            ViewBag.SuiviIRCId = suiviIRCId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AjouterRDV([Bind("SuiviIRCId,Date,ChargeAffaires,Groupe,Affaire,RdvDemande,RdvDetails,PresentsBiat,Lieu,PresentsClient,AutresClientInput,Commentaires")] RDV rdv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rdv);
                await _context.SaveChangesAsync();

                // Mise à jour des statistiques
                await UpdateSuiviIRCStats(rdv.SuiviIRCId);
                return RedirectToAction(nameof(Details), new { id = rdv.SuiviIRCId });
            }
            ViewBag.SuiviIRCId = rdv.SuiviIRCId;
            return View(rdv);
        }

        // ... [Méthodes ModifierRDV, SupprimerRDV existantes] ...

        // COMPTE RENDU ACTIONS

        public async Task<IActionResult> CompteRendu(int? id)
        {
            if (id == null) return NotFound();

            var rdv = await _context.RDVs
                .Include(r => r.CompteRendu)
                .Include(r => r.SuiviIRC)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rdv == null) return NotFound();

            ViewBag.RDVId = id;
            ViewBag.SuiviIRCId = rdv.SuiviIRCId;

            return rdv.CompteRendu == null
                ? RedirectToAction(nameof(CreateCompteRendu), new { rdvId = id })
                : View(rdv.CompteRendu);
        }

        public IActionResult CreateCompteRendu(int rdvId)
        {
            var rdv = _context.RDVs
                .Include(r => r.SuiviIRC)
                .FirstOrDefault(r => r.Id == rdvId);

            if (rdv == null) return NotFound();

            ViewBag.RDVId = rdvId;
            ViewBag.SuiviIRCId = rdv.SuiviIRCId;

            return View(new CompteRendu
            {
                RDVId = rdvId,
                DateCreation = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompteRendu([Bind("RDVId,Contenu,DateCreation")] CompteRendu compteRendu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compteRendu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CompteRendu), new { id = compteRendu.RDVId });
            }

            ViewBag.RDVId = compteRendu.RDVId;
            ViewBag.SuiviIRCId = _context.RDVs
                .Where(r => r.Id == compteRendu.RDVId)
                .Select(r => r.SuiviIRCId)
                .FirstOrDefault();

            return View(compteRendu);
        }

        public async Task<IActionResult> EditCompteRendu(int? id)
        {
            if (id == null) return NotFound();

            var compteRendu = await _context.ComptesRendus
                .Include(c => c.RDV)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compteRendu == null) return NotFound();

            ViewBag.SuiviIRCId = compteRendu.RDV.SuiviIRCId;
            return View(compteRendu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompteRendu(int id, [Bind("Id,RDVId,Contenu,DateCreation")] CompteRendu compteRendu)
        {
            if (id != compteRendu.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compteRendu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompteRenduExists(compteRendu.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(CompteRendu), new { id = compteRendu.RDVId });
            }

            ViewBag.SuiviIRCId = _context.RDVs
                .Where(r => r.Id == compteRendu.RDVId)
                .Select(r => r.SuiviIRCId)
                .FirstOrDefault();

            return View(compteRendu);
        }

        [HttpPost, ActionName("DeleteCompteRendu")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCompteRenduConfirmed(int id)
        {
            var compteRendu = await _context.ComptesRendus
                .Include(c => c.RDV)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compteRendu == null) return NotFound();

            var rdvId = compteRendu.RDV.Id;
            _context.ComptesRendus.Remove(compteRendu);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(CompteRendu), new { id = rdvId });
        }

        // HELPERS
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