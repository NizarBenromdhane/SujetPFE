using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SujetPFE.Models
{
    public class ObjectifCreditViewModel
    {
        public int GroupeId { get; set; }
        public string GroupeNom { get; set; }
        public string Devise { get; set; }
        public decimal? Encours2023TndDat { get; set; }
        public decimal? Encours2023TndDav { get; set; }
        public decimal? Encours2024TndDat { get; set; }
        public decimal? Encours2024TndDav { get; set; }
        public decimal? ObjectifTndDat { get; set; }
        public decimal? ObjectifTndDav { get; set; }
        public string EmployeId { get; set; }
        public int? EmployeResponsableId { get; set; } 
         public List<SelectListItem> ChargesAffaires { get; set; }
    }
}