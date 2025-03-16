using System.ComponentModel.DataAnnotations;

namespace SujetPFE.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Matricule { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }
}