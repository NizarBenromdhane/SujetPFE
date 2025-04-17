using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Domain.Entities
{
    public class ObjectifDepotSubmissionModel
    {
        public int EmployeId { get; set; }
        public int GroupeId { get; set; }
        public decimal? TndDat { get; set; } // Utiliser des types nullable car les champs peuvent être vides lors de la saisie
        public decimal? TndDav { get; set; }
        // Vous pouvez ajouter d'autres propriétés si nécessaire (Devise, Total Tnd, Total Dep)
    }
}
