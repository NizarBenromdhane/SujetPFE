using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Domain.Entities
{
    public class Encours
    {
        [Key]
        public int IdEncours { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeId { get; set; }

        public Employee Employee { get; set; }

        [ForeignKey("Groupe")]
        public int? GroupeId { get; set; }

        public Groupe Groupe { get; set; }

        public string TypeEncours { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Solde { get; set; }

        public DateTime? DateDerniereTransaction { get; set; }

        [Required]
        public int Annee { get; set; }
        public string Devise { get; set; } = "TND"; // Valeur par défaut

        public string Sens { get; set; }
    }
}