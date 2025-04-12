using System;
using System.Collections.Generic;

namespace SujetPFE.Domain.Entities
{
    public class RDV
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ChargeAffaires { get; set; }
        public string Groupe { get; set; }
        public string Affaire { get; set; }
        public string RdvDemande { get; set; }
        public string RdvDetails { get; set; }
        public string PresentsBiat { get; set; }
        public string Lieu { get; set; }
        public string PresentsClient { get; set; }
        public string AutresClientInput { get; set; }
        public string Commentaires { get; set; }

        // Propriétés manquantes à ajouter
        public int NombreVisites { get; set; }  // Ajouté pour résoudre l'erreur
        public int ClientId { get; set; }       // Clé étrangère vers Client
        public Client Client { get; set; }      // Navigation property vers Client

        // Relations existantes
        public int SuiviIRCId { get; set; }
        public SuiviIRC SuiviIRC { get; set; }
        public CompteRendu CompteRendu { get; set; }
        public List<ObjetVisite> ObjetsVisite { get; set; } = new List<ObjetVisite>();
    }
}