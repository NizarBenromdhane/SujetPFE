using Microsoft.AspNetCore.Mvc;
using SujetPFE.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SujetPFE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;

namespace SujetPFE.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(SignInManager<Utilisateur> signInManager, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Matricule, model.MotDePasse, model.RememberMe, lockoutOnFailure: false);
                Console.Write(model.Matricule+ " // " + model.MotDePasse);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Matricule ou mot de passe incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> TestConnexion()
        {
            try
            {
                var utilisateurs = await _context.Utilisateurs.ToListAsync();
                return Ok($"Connexion réussie. Nombre d'utilisateurs : {utilisateurs.Count}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur de connexion : {ex.Message}");
            }
        }
    }
}