namespace SujetPFE.Domain.Entities
{
    public class HistoriqueObjectif
    {
        public int Id { get; set; }  
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int ObjectifId { get; set; } 
        public Objectif Objectif { get; set; }
    }
}
