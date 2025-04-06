namespace SujetPFE.Domain.Entities;

public class Client
{
    public string IDString { get; set; } // Assuming ID is a string, adjust if it's an int
    public int Id { get; set; }
    public string RaisonSociale { get; set; }
    public int GroupeId { get; set; }
    public string Charge { get; set; }
    public string Activite { get; set; }
    public string SousActivite { get; set; }
    public string PP { get; set; }
    public string Segments { get; set; }
    public DateTime AjouteLe { get; set; }
    public string Pole { get; set; }
    public string RC { get; set; }
    public string CTX { get; set; }
    public DateTime? SortieLe { get; set; }

    public Groupe Groupe { get; set; }
    public ICollection<Objective> Objectifs { get; set; }
}
