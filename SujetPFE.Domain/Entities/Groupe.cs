namespace SujetPFE.Domain.Entities
{
    public class Groupe
    {
        public int Id { get; set; }
        public string IdBct { get; set; } 
        public string Nom { get; set; }
        public string Charge { get; set; }  
        public DateTime AjouteLe { get; set; }  
        public string PartieLiee { get; set; }  
    }
}

