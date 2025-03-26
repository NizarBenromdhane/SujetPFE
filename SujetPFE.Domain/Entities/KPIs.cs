using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Models
{
    public class KPIValue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string KPI { get; set; } // Nom du KPI

        [Required]
        public string Valeur { get; set; } // Valeur du KPI

        [ForeignKey("Pac")] // Clé étrangère vers le PAC
        public int PacId { get; set; }
        public Pac Pac { get; set; }
    }
}