using System;
using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Models
{
    public class Pac
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis.")]
        public string Titre { get; set; }

        [Required(ErrorMessage = "La description est requise.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La date de début est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "Le responsable est requis.")]
        public string Responsable { get; set; }

        [Required(ErrorMessage = "Le statut est requis.")]
        public string Statut { get; set; }

        [Required(ErrorMessage = "Le groupe est requis.")]
        public string Groupe { get; set; }

        [Required(ErrorMessage = "L'affaire est requise.")]
        public string Affaire { get; set; }

        [Required(ErrorMessage = "Le KPL est requis.")]
        public string KPL { get; set; }

        [Required(ErrorMessage = "La priorité est requise.")] // Ajout de l'attribut Required
        public string Priority { get; set; }

        public string Recommandations { get; set; }
        public string Limites { get; set; }
    }
}