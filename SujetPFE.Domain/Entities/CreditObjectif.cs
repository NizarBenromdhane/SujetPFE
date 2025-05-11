namespace SujetPFE.Domain.Entities
{
    public class CreditObjectif
    {
        public int Id { get; set; }
        public string Periode { get; set; }
        public string TypeCredit { get; set; }
        public decimal MontantObjectif { get; set; }
        public int? EmployeId { get; set; }
        public Employee Employe { get; set; }
        public int? GroupeId { get; set; }
        public Groupe Groupe { get; set; }

        // Nouvelle propriété pour l'année
        public int Annee { get; set; }
    }
}// Nouveau modèle pour la réception des objectifs par groupe
public class ObjectifsGroupe
{
    public decimal? Objectif2025Dat { get; set; }
    public decimal? Objectif2025Dav { get; set; }
}