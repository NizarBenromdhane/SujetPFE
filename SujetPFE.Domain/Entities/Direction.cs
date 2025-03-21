using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Domain.Entities;

public class Direction
{
    public int Id { get; set; }
    [MaxLength(10)]
    public string Libelle { get; set; }
    public string Pole { get; set; }
    public string Shortname { get; set; }
    public virtual ICollection<Employe> Employes { get; set; } = new List<Employe>();
}
