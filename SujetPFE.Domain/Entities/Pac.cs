using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Models
{
    public class Pac
    {
        public int Id { get; set; }
        [Required]
        public string Titre { get; set; }
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }
        [Required]
        public string Responsable { get; set; }
        public string Statut { get; set; }
        public string Groupe { get; set; }
        public string Affaire { get; set; }
        public string KPL { get; set; }
        public string Recommandations { get; set; }
        public string Limites { get; set; }

        public List<KPIValue> KPIValues { get; set; } // Propriété de navigation pour les KPIs
    }
}
