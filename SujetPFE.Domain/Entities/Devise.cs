using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SujetPFE.Domain.Entities
{
    public class Devise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(3)]
        public string Code { get; set; }  // Ex: "TND", "EUR", "USD"

        public string Libelle { get; set; }  // Ex: "Dinar tunisien"

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TauxChange { get; set; }  // Optionnel: taux de change par rapport à une devise de référence
    }
}