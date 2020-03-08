using System.ComponentModel.DataAnnotations;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class ConfirmEmailViewModel
    {
        [Required]
        [Display(Name = "Identificador do Usuário")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Código de Ativação")]
        public string Code { get; set; }
    }
}