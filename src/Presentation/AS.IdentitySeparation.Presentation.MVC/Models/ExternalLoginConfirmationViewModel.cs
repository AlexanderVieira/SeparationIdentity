using System.ComponentModel.DataAnnotations;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}