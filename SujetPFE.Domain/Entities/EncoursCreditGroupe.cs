using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SujetPFE.Domain.Entities
{
    public class EncoursCreditGroupe
    {
        [Key]
        public int Id { get; set; } // Clé primaire

        [ForeignKey("Groupe")]
        public int GroupeId { get; set; }

        public int Annee { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Montant { get; set; }

        // Navigation property vers l'entité Groupe (si vous avez une relation)
        public Groupe Groupe { get; set; }
    }
}