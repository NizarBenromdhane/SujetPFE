using SujetPFE.Domain.Entities;
using System.ComponentModel.DataAnnotations;

public class ObjetVisite
{
    [Key] // Attribut pour spécifier la clé primaire
    public int Id { get; set; }

    public string Orientation { get; set; }
    public string Action { get; set; }
    public string Objet { get; set; }

    // Clé étrangère vers RDV si nécessaire
    public int RDVId { get; set; }
    public RDV RDV { get; set; }
}