using System;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SujetPFE.Domain.Entities
{
    public class ObjectifSuivi
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'objectif est obligatoire.")]
        public int ObjectifId { get; set; }

        [Required(ErrorMessage = "La réalisation est obligatoire.")]
        public decimal Realisation { get; set; }

        public decimal Ecart { get; set; }

        [Required(ErrorMessage = "La date de suivi est obligatoire.")]
        public DateTime DateSuivi { get; set; }

        public Objective Objective { get; set; } // Propriété de navigation
    }
}