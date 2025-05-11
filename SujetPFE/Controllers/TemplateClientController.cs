using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;

namespace SujetPFE.Controllers
{
    public class TemplateClientController : Controller
    {
        private readonly PcbContext _context;
        private readonly IAntiforgery _antiforgery;

        public TemplateClientController(PcbContext context, IAntiforgery antiforgery)
        {
            _context = context;
            _antiforgery = antiforgery;
        }

        // GET: TemplateClients
        public IActionResult Index()
        {
            return View(); // La récupération des données se fera par JavaScript
        }

        // GET: TemplateClients/GetTemplates
        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {
            var templates = await _context.TemplateClients.ToListAsync();
            return Json(templates);
        }

        // GET: TemplateClients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templateClient = await _context.TemplateClients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (templateClient == null)
            {
                return NotFound();
            }

            return View(templateClient);
        }

        // GET: TemplateClients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TemplateClients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Email,Telephone,DateCreation")] TemplateClient templateClient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(templateClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(templateClient);
        }

        // GET: TemplateClients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templateClient = await _context.TemplateClients.FindAsync(id);
            if (templateClient == null)
            {
                return NotFound();
            }
            return View(templateClient);
        }

        // POST: TemplateClients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Email,Telephone,DateCreation")] TemplateClient templateClient)
        {
            if (id != templateClient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(templateClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplateClientExists(templateClient.Id))
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
            return View(templateClient);
        }

        // POST: TemplateClients/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var templateClient = await _context.TemplateClients.FindAsync(id);
            if (templateClient != null)
            {
                _context.TemplateClients.Remove(templateClient);
                await _context.SaveChangesAsync();
                return Ok(); // Retourne un statut 200 OK pour indiquer le succès
            }
            return NotFound(); // Retourne un statut 404 Not Found si le modèle n'existe pas
        }

        private bool TemplateClientExists(int id)
        {
            return _context.TemplateClients.Any(e => e.Id == id);
        }
    }
}