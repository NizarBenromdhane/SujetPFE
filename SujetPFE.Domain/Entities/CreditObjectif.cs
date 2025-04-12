using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Domain.Entities
{
    public class CreditObjectif
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La période est obligatoire.")]
        public string Periode { get; set; } // Corrected to string

        [Required(ErrorMessage = "Le type de crédit est obligatoire.")]
        public string TypeCredit { get; set; } // Corrected to string

        [Required(ErrorMessage = "Le montant objectif est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
        public decimal MontantObjectif { get; set; } // Corrected to decimal

        public int? EmployeId { get; set; } // Corrected to nullable int (if optional)
        public int? GroupeId { get; set; }   // Corrected to nullable int (if optional)
        // Ajoutez d'autres propriétés si nécessaire
    }
}