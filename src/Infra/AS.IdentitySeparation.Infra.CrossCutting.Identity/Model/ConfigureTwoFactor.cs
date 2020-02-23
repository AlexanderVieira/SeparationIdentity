using System.Collections.Generic;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class ConfigureTwoFactor
    {
        public string SelectedProvider { get; set; }
        public ICollection<string> Providers { get; set; }
    }
}