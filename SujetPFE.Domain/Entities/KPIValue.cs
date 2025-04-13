using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Models
{
    public class KPIValue
    {
        public int Id { get; set; }
        public string Name { get; set; } // Example property for KPI name
        public string Value { get; set; } // Example property for KPI value

        [ForeignKey("Pac")]
        public int PacId { get; set; }
        public Pac Pac { get; set; }
    }
}