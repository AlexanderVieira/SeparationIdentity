using System.Collections.Generic;
using System.Web.Mvc;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class SendCode
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }
}