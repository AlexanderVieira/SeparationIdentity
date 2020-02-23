using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Collections.Generic;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Model
{
    public class ManageLogins
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}