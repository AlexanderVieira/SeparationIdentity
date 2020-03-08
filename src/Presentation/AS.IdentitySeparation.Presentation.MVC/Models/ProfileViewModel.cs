using System;
using System.ComponentModel.DataAnnotations;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class ProfileViewModel
    {
        
        public Guid ProfileVmId { get; set; }
        
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O Campo Nome é obrigatório!")]
        [StringLength(150, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 4)]
        public String FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "O Campo Sobrenome é obrigatório!")]
        [StringLength(150, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres.", MinimumLength = 4)]
        public String LastName { get; set; }

        [EmailAddress]
        [Display(Name = "E-mail")]
        public String Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Foto")]
        public String PhotoUrl { get; set; }
        
    }
}