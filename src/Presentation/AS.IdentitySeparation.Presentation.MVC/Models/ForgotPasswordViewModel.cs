using System.ComponentModel.DataAnnotations;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
