using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SujetPFE.Data; // Assurez-vous d'ajouter le namespace de votre DbContext

namespace SujetPFE.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Logique pour récupérer les données du tableau de bord
            // Exemple : var donnees = _context.DonneesTableauDeBord.ToList();

            return View();
        }
    }
}