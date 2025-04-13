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
    public class TemplateClientController : Controller
    {
        private readonly PcbContext _context;

        public TemplateClientController(PcbContext context)
        {
            _context = context;
        }

        // GET: TemplateClients
        public async Task<IActionResult> Index()
        {
            return View(await _context.TemplateClients.ToListAsync());
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Email,Telephone")] TemplateClient templateClient)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Email,Telephone")] TemplateClient templateClient)
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

        // GET: TemplateClients/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: TemplateClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var templateClient = await _context.TemplateClients.FindAsync(id);
            if (templateClient != null)
            {
                _context.TemplateClients.Remove(templateClient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemplateClientExists(int id)
        {
            return _context.TemplateClients.Any(e => e.Id == id);
        }
    }
}