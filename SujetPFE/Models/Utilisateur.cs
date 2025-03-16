using Microsoft.AspNetCore.Identity;

namespace SujetPFE.Models
{
    public class Utilisateur : IdentityUser
    {
        public string Matricule { get; set; }
        // Ajoutez d'autres propriétés si nécessaire
    }
}