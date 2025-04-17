using SujetPFE.Domain.Entities;

namespace SujetPFE.Models
{
    public class ObjectifDepotViewModel
    {
        public int GroupeId { get; set; }
        public string GroupeLibelle { get; set; }
        public Groupe Groupe { get; set; }

        public decimal Encours2023_TndDat { get; set; }
        public decimal Encours2023_TndDav { get; set; }
        public decimal Encours2023_TotalDep => Encours2023_TndDat + Encours2023_TndDav;

        public decimal Encours2024_TndDat { get; set; }
        public decimal Encours2024_TndDav { get; set; }
        public decimal Encours2024_TotalDep => Encours2024_TndDat + Encours2024_TndDav;

        public Objectif2025ViewModel Objectif2025 { get; set; }

        public int? EmployeeId { get; set; }
    }

    public class Objectif2025ViewModel
    {
        public decimal? MontantDat { get; set; }
        public decimal? MontantDav { get; set; }
        public decimal? TotalTnd => MontantDat + MontantDav;
        public decimal? TotalDep { get; set; } // Vous devrez peut-être ajuster cela en fonction de votre logique
    }
}