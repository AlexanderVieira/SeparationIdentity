using System.Collections.Generic;

namespace AS.IdentitySeparation.Presentation.MVC.Models
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<string> Providers { get; set; }
    }
}