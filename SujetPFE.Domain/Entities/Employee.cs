
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Matricule1 { get; set; }
    public string Matricule_t24_2 { get; set; }
    public string Nom { get; set; }
    public string Profil { get; set; }
    public string Statut { get; set; }
    public int DirectionId { get; set; }

    public virtual Direction Direction { get; set; } 
}
