using System;
using System.Collections.Generic;

namespace SujetPFE.Domain.Entities
{
    public class SuiviIRC
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime DateVisite { get; set; }
        public string ChargeAffaires { get; set; }
        public int NombreVisites { get; set; }
        public int NombreRDV { get; set; }
        public double MoyenneRDV { get; set; }
        public string Commentaires { get; set; }

        // Relations
        public Client Client { get; set; }
        public List<RDV> RDVs { get; set; } = new List<RDV>();
    }
}