using System.Collections.Generic;
using System.Web.Mvc;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}