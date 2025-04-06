using System;

namespace SujetPFE.Domain.Entities
{
    public class RDV
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Client { get; set; }
        public string Commentaires { get; set; }
        public int NombreVisites { get; set; }

        // Autres propriétés pertinentes pour un RDV
    }
}