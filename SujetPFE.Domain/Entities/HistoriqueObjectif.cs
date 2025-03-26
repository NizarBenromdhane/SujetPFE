using SujetPFE.Domain.Entities; // Add this using directive

public class HistoriqueObjectif
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public int ObjectifId { get; set; }
    public Objective Objective { get; set; }
}