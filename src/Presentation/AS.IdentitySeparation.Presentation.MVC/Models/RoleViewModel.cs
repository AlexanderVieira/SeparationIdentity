﻿using System.ComponentModel.DataAnnotations;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome da Role")]
        public string Name { get; set; }
    }
}