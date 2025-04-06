using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SujetPFE.Domain.Entities
{
    public class SuiviIRC
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client is required.")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required(ErrorMessage = "Date de la visite is required.")]
        public DateTime DateVisite { get; set; }

        [Required(ErrorMessage = "Charge d'affaires is required.")]
        [StringLength(255, ErrorMessage = "Charge d'affaires cannot exceed 255 characters.")]
        public string ChargeAffaires { get; set; }

        [Required(ErrorMessage = "Nombre de visites is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Nombre de visites must be a non-negative number.")]
        public int NombreVisites { get; set; }

        [Required(ErrorMessage = "Nombre de RDV is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Nombre de RDV must be a non-negative number.")]
        public int NombreRDV { get; set; }

        public double MoyenneRDV { get; set; }

        public string Commentaires { get; set; }
    }
}