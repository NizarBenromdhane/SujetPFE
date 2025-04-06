using System;
using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Domain.Entities
{
    public class ObjectifCreditDepot
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le type d'objectif est obligatoire.")]
        public string TypeObjectif { get; set; } // "Crédit" ou "Dépôt"

        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
        public decimal Montant { get; set; }

        [Required(ErrorMessage = "La date de début est obligatoire.")]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire.")]
        public DateTime DateFin { get; set; }

        // Ajoutez d'autres propriétés si nécessaire
    }
}