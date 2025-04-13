using System;
using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Domain.Entities // Assurez-vous que le namespace correspond à votre projet
{
    public class TemplateClient // Un nom de modèle spécifique à la vue peut être utile
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