using AS.IdentitySeparation.Infra.CrossCutting.Identity.Configuration;
using AS.IdentitySeparation.Infra.CrossCutting.Identity.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Owin;
using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

//[assembly: OwinStartup(typeof(AS.IdentitySeparation.Application.API.App_Start.Startup))]
namespace AS.IdentitySeparation.Application.API.App_Start
{
    public partial class Startup
    {
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";        

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            var Secret = ConfigurationManager.AppSettings["SecretKey"];            
            byte[] key = Convert.FromBase64String(Secret);
            
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<ApplicationUserManager>());

            // Configure the db context, user manager and role manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie 

            app.UseJwtBearerAuthentication(
               new Microsoft.Owin.Security.Jwt.JwtBearerAuthenticationOptions
               {
                   AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                   TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                   {
                       //ValidateIssuer = true,
                       //ValidateAudience = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = ConfigurationManager.AppSettings["ValidIssuer"],
                       ValidAudience = ConfigurationManager.AppSettings["ValidAudience"],
                       ValidateLifetime = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key)
                   }

               });

            //CookieAuthentication.EnableCookieAuthentication(app);

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers

            app.UseMicrosoftAccountAuthentication(
                clientId: "SEU ID",
                clientSecret: "SEU TOKEN");

            app.UseTwitterAuthentication(
               consumerKey: "SEU ID",
               consumerSecret: "SEU TOKEN");


            app.UseGoogleAuthentication(
                clientId: "SEU ID",
                clientSecret: "SEU TOKEN");


            var fao = new FacebookAuthenticationOptions
            {
                AppId = "SEU ID",
                AppSecret = "SEU TOKEN"
            };

            fao.Scope.Add("email");
            fao.Scope.Add("publish_actions");
            fao.Scope.Add("basic_info");

            fao.Provider = new FacebookAuthenticationProvider()
            {

                OnAuthenticated = (context) =>
                {
                    context.Identity.AddClaim(new System.Security.Claims.Claim("urn:facebook:access_token", context.AccessToken, XmlSchemaString, "Facebook"));
                    foreach (var x in context.User)
                    {
                        var claimType = string.Format("urn:facebook:{0}", x.Key);
                        string claimValue = x.Value.ToString();
                        if (!context.Identity.HasClaim(claimType, claimValue))
                            context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, XmlSchemaString, "Facebook"));

                    }
                    return Task.FromResult(0);
                }
            };

            fao.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            app.UseFacebookAuthentication(fao);
        }
        
    }
}