namespace SujetPFE.Domain.Entities;

public class Objective
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public float ValeurCible { get; set; }
    public float ValeurActuelle { get; set; }
    public string UniteMesure { get; set; }
    public string Statut { get; set; }
    public string Matricule1 { get; set; }
    public int ClientId { get; set; }
    public float MontantCredit { get; set; }
    public float MontantDepot { get; set; }
    public string TypeProduit { get; set; }
    public int EmployeeId { get; set; }
    public Client Client { get; set; }
    public Employee Employee { get; set; }
}