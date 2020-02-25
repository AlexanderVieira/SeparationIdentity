using AS.IdentitySeparation.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Filters
{
    public static class CookieAuthentication
    {
        public static void EnableCookieAuthentication(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  

                    //OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                    //    validateInterval: TimeSpan.FromMinutes(30),
                    //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))

                    OnValidateIdentity = ApplicationCookieIdentityValidator.OnValidateIdentity(
                       validateInterval: TimeSpan.FromMinutes(0),
                       regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
        }
    }
}