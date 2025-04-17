using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SujetPFE.Domain.Entities
{
    public class PratiquesManagériales // Un nom de modèle spécifique à la vue peut être utile
    {
        public int Id { get; set; }

        [Display(Name = "Nom du Client")]
        public string Nom { get; set; }

        [Display(Name = "Adresse Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Téléphone")]
        public string Telephone { get; set; }

        // Ajoutez d'autres propriétés pertinentes pour votre interface client
    }
}
