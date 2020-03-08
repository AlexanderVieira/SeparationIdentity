using System.ComponentModel.DataAnnotations;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}