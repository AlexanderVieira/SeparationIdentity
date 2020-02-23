using AS.IdentitySeparation.Infra.CrossCutting.Identity.Configuration;
using AS.IdentitySeparation.Infra.CrossCutting.Identity.Context;
using AS.IdentitySeparation.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;

namespace AS.IdentitySeparation.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {
            container.RegisterPerWebRequest<ApplicationDbContext>();
            container.RegisterPerWebRequest<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()));
            container.RegisterPerWebRequest<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole>());
            container.RegisterPerWebRequest<ApplicationRoleManager>();
            container.RegisterPerWebRequest<ApplicationUserManager>();
            container.RegisterPerWebRequest<ApplicationSignInManager>();

            //container.RegisterPerWebRequest<IUsuarioRepository, UsuarioRepository>();
        }
    }
}