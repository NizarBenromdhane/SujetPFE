using System.ComponentModel.DataAnnotations;


namespace SujetPFE.Domain.Entities
{
    public class CreditObjectif
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La période est obligatoire.")]
        public string Periode { get; set; }

        [Required(ErrorMessage = "Le type de crédit est obligatoire.")]
        public string TypeCredit { get; set; }

        [Required(ErrorMessage = "Le montant objectif est obligatoire.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Le montant doit être supérieur à zéro.")]
        public decimal MontantObjectif { get; set; }

        public int? EmployeId { get; set; }
        public int? GroupeId { get; set; }
        // Ajoutez d'autres propriétés si nécessaire
    }
}