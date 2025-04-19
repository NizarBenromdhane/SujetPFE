using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // N'oubliez pas d'ajouter cette directive using

namespace SujetPFE.Domain.Entities;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Column("Matricule1")]
    public string Matricule1 { get; set; }

    [Column("Matricule_t24_2")]
    public string MatriculeT24_2 { get; set; }

    public string Nom { get; set; }

    public string Profil { get; set; }

    public string Statut { get; set; }

    [ForeignKey("Direction")]
    public int DirectionId { get; set; }

    public Direction Direction { get; set; }

    // Navigation property pour les Encours
    public ICollection<Encours> Encours { get; set; }

    public string Fonction { get; set; } // Ajout de la propriété Fonction

    // Collection des groupes dont cet employé est responsable
    public ICollection<Groupe> GroupesResponsables { get; set; }
}