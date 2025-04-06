using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Domain.Entities;
using SujetPFE.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using SujetPFE.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SujetPFE.Controllers
{
    public class ClientsController : Controller
    {
        private readonly PcbContext _context;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(PcbContext context, ILogger<ClientsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var pcbContext = _context.Clients.Include(c => c.Groupe);
            return View(await pcbContext.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Groupe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            // Charger les groupes pour la liste déroulante
            ViewData["GroupeId"] = new SelectList(_context.Groupes, "Id", "Nom"); // Remplacez "Nom" par la propriété d'affichage

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RaisonSociale,GroupeId,Charge,Activite,SousActivite,PP,Segments,AjouteLe,Pole,RC,CTX,SortieLe")] Client client)
        {
            // Validation côté serveur pour GroupeId
            if (!_context.Groupes.Any(g => g.Id == client.GroupeId))
            {
                ModelState.AddModelError("GroupeId", "Le groupe sélectionné n'existe pas.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recharger les groupes en cas d'erreur
            ViewData["GroupeId"] = new SelectList(_context.Groupes, "Id", "Nom", client.GroupeId); // Remplacez "Nom" par la propriété d'affichage

            // Log ModelState errors
            foreach (var modelStateKey in ModelState.Keys)
            {
                var modelStateVal = ModelState[modelStateKey];
                foreach (var error in modelStateVal.Errors)
                {
                    _logger.LogError($"ModelState Error: {modelStateKey} - {error.ErrorMessage}");
                }
            }

            // Log client values
            _logger.LogInformation($"Client values: {JsonSerializer.Serialize(client)}");

            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            ViewData["GroupeId"] = new SelectList(_context.Groupes, "Id", "Nom", client.GroupeId); // Remplacez "Nom" par la propriété d'affichage
            return View(client);
        }

        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RaisonSociale,GroupeId,Charge,Activite,SousActivite,PP,Segments,AjouteLe,Pole,RC,CTX,SortieLe")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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

            ViewData["GroupeId"] = new SelectList(_context.Groupes, "Id", "Nom", client.GroupeId); // Remplacez "Nom" par la propriété d'affichage
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Groupe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

        // POST: Client/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select an Excel file");
                return View();
            }

            try
            {
                // Verify file extension
                if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("", "Please upload a valid .xlsx file");
                    return View();
                }

                // Create temp file path
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var filePath = Path.Combine(uploadsFolder, excelFile.FileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await excelFile.CopyToAsync(stream);
                }

                // Process the file
                var mapper = new ExcelToClientMapper();
                List<Client> clients = mapper.MapClientsFromExcel(filePath);

                // Here you would typically save to database
                // For example with Entity Framework Core:
                 _context.Clients.AddRange(clients);
                 await _context.SaveChangesAsync();

                // Clean up
                System.IO.File.Delete(filePath);

                ViewBag.Message = $"Successfully processed {clients.Count} clients";
                return View("Index", clients);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error processing file: {ex.Message}");
                return View();
            }
        }
    }
    }