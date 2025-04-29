using SujetPFE.Domain.Entities;

namespace SujetPFE.Models
{
    public class ObjectifDepotViewModel
    {
        public Groupe Groupe { get; set; } // Make sure you have this property
        public int GroupeId { get; set; }
        public string GroupeNom { get; set; }
        public string Devise { get; set; }

        // Encours 2023
        public decimal? Encours2023Dat { get; set; }
        public decimal? Encours2023Dav { get; set; }
        public decimal Encours2023Total { get; set; }

        // Encours 2024
        public decimal? Encours2024Dat { get; set; }
        public decimal? Encours2024Dav { get; set; }
        public decimal Encours2024Total { get; set; }
        public decimal? Evolution2024 { get; set; }

        // Objectifs 2025
        public Objectif2025ViewModel Objectif2025 { get; set; } // Ensure this nested object is present
        public decimal Objectif2025Total { get; set; }
        public decimal? Evolution2025 { get; set; }

        public int? EmployeResponsableId { get; set; } // Ensure this property exists
    }

    public class Objectif2025ViewModel
    {
        public decimal? MontantDat { get; set; }
        public decimal? MontantDav { get; set; }
        public decimal TotalDep => (MontantDat ?? 0) + (MontantDav ?? 0);
    }
}