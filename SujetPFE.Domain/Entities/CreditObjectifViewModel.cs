namespace SujetPFE.Models
{
    public class CreditObjectifViewModel
    {
        public int? GroupeId { get; set; }
        public string GroupeNom { get; set; }
        public string Devise { get; set; } = "TND";

        // Encours 2023
        public decimal? Encours2023Dat { get; set; }
        public decimal? Encours2023Dav { get; set; }
        public decimal? Encours2023TotalTnd { get; set; }
        public decimal? Encours2023TndDat { get; set; }
        public decimal? Encours2023TndDav { get; set; }
        public decimal? Encours2023TotalDep { get; set; }
        public decimal? Encours2023Total { get; set; } // Added this!

        // Encours 2024
        public decimal? Encours2024Dat { get; set; }
        public decimal? Encours2024Dav { get; set; }
        public decimal? Encours2024TotalTnd { get; set; }
        public decimal? Encours2024TndDat { get; set; }
        public decimal? Encours2024TndDav { get; set; }
        public decimal? Encours2024TotalDep { get; set; }
        public decimal? Encours2024Total { get; set; } // Added this!

        // Objectifs 2025
        public decimal? Objectif2025Dat { get; set; }
        public decimal? Objectif2025Dav { get; set; }
        public decimal? Objectif2025TotalDep { get; set; }
        public decimal? Objectif2025TotalTnd { get; set; }
        public decimal? Objectif2025Tiro { get; set; }
        public decimal? Objectif2025TndDat { get; set; }
        public decimal? Objectif2025TndDav { get; set; }
        public decimal? Objectif2025Total { get; set; } // Added this!

        // Informations supplémentaires (à évaluer si nécessaires ici)
        public int Id { get; set; }

        // Nouvelle propriété calculée
        public int DeviseId { get; set; }
        public string Periode { get; set; }
        public string TypeCredit { get; set; }
        public decimal MontantObjectif { get; set; }
        public int? EmployeId { get; set; }
        public string EmployeNom { get; set; }
        public int? EmployeResponsableId { get; set; }
    }
}