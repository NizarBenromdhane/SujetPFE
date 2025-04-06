using System.ComponentModel.DataAnnotations;

public class PlanAction
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre est requis.")]
    [Display(Name = "Titre")]
    public string Titre { get; set; }

    [Display(Name = "Date de début")]
    public DateTime DateDebut { get; set; }

    [Display(Name = "Date de fin")]
    public DateTime DateFin { get; set; }

    [Display(Name = "Responsable")]
    public string Responsable { get; set; }

    [Display(Name = "Statut")]
    public string Statut { get; set; }

    // Autres propriétés
}