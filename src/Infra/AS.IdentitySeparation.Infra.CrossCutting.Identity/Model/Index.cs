using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class Index
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
}