using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Domain.Entities
{
    public class CompteRendu
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "RDV")]
        public int RDVId { get; set; }
        [ForeignKey("RDVId")]
        public RDV RDV { get; set; }
        [Display(Name = "Date de Création")]
        public DateTime? DateCreation { get; set; } // Make it nullable
        [Display(Name = "Contenu")]
        [DataType(DataType.MultilineText)]
        public string Contenu { get; set; }
    }
}