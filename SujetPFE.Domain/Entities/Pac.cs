using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Models
{
    public class Pac
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le titre est requis.")]
        public string Titre { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "La date de début est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        [Required(ErrorMessage = "Le responsable est requis.")]
        public string Responsable { get; set; }

        public string Statut { get; set; }
        public string Groupe { get; set; }
        public string Affaire { get; set; }
        public string KPL { get; set; }
        public string Recommandations { get; set; }
        public string Limites { get; set; }

        public List<KPIValue> KPIValues { get; set; }
    }
}