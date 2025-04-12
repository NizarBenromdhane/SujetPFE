using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;
using SujetPFE.Services; // Assurez-vous que le namespace de vos services est correct
using Microsoft.Extensions.Configuration; // Pour accéder à la configuration

namespace SujetPFE.Controllers
{
    public class ObjectivesController : Controller
    {
        private readonly PcbContext _context;
        private readonly ILogger<ObjectivesController> _logger;
        private readonly ExcelToClientMapper _excelToClientMapper; // Injecter ExcelToClientMapper
        private readonly IConfiguration _configuration; // Injecter IConfiguration

        public ObjectivesController(PcbContext context, ILogger<ObjectivesController> logger, ExcelToClientMapper excelToClientMapper, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _excelToClientMapper = excelToClientMapper;
            _configuration = configuration;
        }

        // GET: Objectives
        [HttpGet("/Objectives")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var objectives = await _context.Objectives
                    .Include(o => o.Client)
                    .Include(o => o.Employee)
                    .ToListAsync();

                return View(objectives);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving objectives in Index action.");
                return View("Error", new SujetPFE.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // GET: Objectives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var objective = await _context.Objectives
                    .Include(o => o.Client)
                    .Include(o => o.Employee)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (objective == null)
                {
                    return NotFound();
                }

                return View(objective);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving objective with ID {id} in Details action.");
                return View("Error", new SujetPFE.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // GET: Objectives/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale");

            // Récupérer la liste des employés depuis Excel
            string employesExcelPath = _configuration["CheminFichierEmployesExcel"];
            List<SelectListItem> employesFromExcel = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(employesExcelPath) && System.IO.File.Exists(employesExcelPath))
            {
                employesFromExcel = _excelToClientMapper.MapEmployesFromExcel(employesExcelPath);
            }
            ViewData["EmployeeId"] = new SelectList(employesFromExcel, "Value", "Text");

            return View();
        }

        // POST: Objectives/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,DateDebut,DateFin,ValeurCible,ValeurActuelle,UniteMesure,Statut,Matricule1,ClientId,MontantCredit,MontantDepot,TypeProduit,EmployeeId")] Objective objective)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(objective);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database error creating objective.");
                    ModelState.AddModelError("", "Unable to save changes. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating objective.");
                    ModelState.AddModelError("", "An unexpected error occurred.");
                }
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale", objective.ClientId);

            // Repopuler la liste des employés en cas d'erreur
            string employesExcelPath = _configuration["CheminFichierEmployesExcel"];
            List<SelectListItem> employesFromExcel = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(employesExcelPath) && System.IO.File.Exists(employesExcelPath))
            {
                employesFromExcel = _excelToClientMapper.MapEmployesFromExcel(employesExcelPath);
            }
            ViewData["EmployeeId"] = new SelectList(employesFromExcel, "Value", "Text", objective.EmployeeId);

            return View(objective);
        }

        // GET: Objectives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var objective = await _context.Objectives.FindAsync(id);
                if (objective == null)
                {
                    return NotFound();
                }
                ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale", objective.ClientId);

                // Récupérer la liste des employés depuis Excel pour l'édition (si nécessaire)
                string employesExcelPath = _configuration["CheminFichierEmployesExcel"];
                List<SelectListItem> employesFromExcel = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(employesExcelPath) && System.IO.File.Exists(employesExcelPath))
                {
                    employesFromExcel = _excelToClientMapper.MapEmployesFromExcel(employesExcelPath);
                }
                ViewData["EmployeeId"] = new SelectList(employesFromExcel, "Value", "Text", objective.EmployeeId);

                return View(objective);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving objective with ID {id} for editing.");
                return View("Error", new SujetPFE.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // POST: Objectives/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,DateDebut,DateFin,ValeurCible,ValeurActuelle,UniteMesure,Statut,Matricule1,ClientId,MontantCredit,MontantDepot,TypeProduit,EmployeeId")] Objective objective)
        {
            if (id != objective.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(objective);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectiveExists(objective.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error editing objective with ID {id}.");
                    ModelState.AddModelError("", "An unexpected error occurred.");
                }
                return View(objective);
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "RaisonSociale", objective.ClientId);

            // Repopuler la liste des employés en cas d'erreur
            string employesExcelPath = _configuration["CheminFichierEmployesExcel"];
            List<SelectListItem> employesFromExcel = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(employesExcelPath) && System.IO.File.Exists(employesExcelPath))
            {
                employesFromExcel = _excelToClientMapper.MapEmployesFromExcel(employesExcelPath);
            }
            ViewData["EmployeeId"] = new SelectList(employesFromExcel, "Value", "Text", objective.EmployeeId);

            return View(objective);
        }

        // GET: Objectives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var objective = await _context.Objectives
                    .Include(o => o.Client)
                    .Include(o => o.Employee)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (objective == null)
                {
                    return NotFound();
                }

                return View(objective);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving objective with ID {id} for deletion.");
                return View("Error", new SujetPFE.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // POST: Objectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var objective = await _context.Objectives.FindAsync(id);
                if (objective != null)
                {
                    _context.Objectives.Remove(objective);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting objective with ID {id}.");
                return View("Error", new SujetPFE.Models.ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        private bool ObjectiveExists(int id)
        {
            return _context.Objectives.Any(e => e.Id == id);
        }
    }
}