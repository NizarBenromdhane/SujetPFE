using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Domain.Entities
{
    public class ObjectifCreditDepot
    {
        [Key]
        public int Id { get; set; }

        public int GroupeId { get; set; }

        [ForeignKey("GroupeId")]
        public Groupe Groupe { get; set; }

        [Required]
        public decimal Montant { get; set; } // Vous pourriez choisir de le rendre nullable si MontantDat/Dav sont obligatoires

        public decimal? MontantDat { get; set; } // Ajout de MontantDat
        public decimal? MontantDav { get; set; } // Ajout de MontantDav

        [Required]
        public string TypeObjectif { get; set; }

        [Required]
        public DateTime DateDebut { get; set; }

        [Required]
        public DateTime DateFin { get; set; }

        public int? EmployeId { get; set; }

        [ForeignKey("EmployeId")]
        public Employee Employe { get; set; }

        // Ajout de la propriété Annee
        [Required]
        public int Annee { get; set; }
    }
}