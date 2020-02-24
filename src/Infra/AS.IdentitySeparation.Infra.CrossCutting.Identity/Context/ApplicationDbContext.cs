using AS.IdentitySeparation.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDisposable
    {
        public ApplicationDbContext() : base("ConnStrIdentity", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, AS.IdentitySeparation.Infra.CrossCutting.Identity.Migrations.Configuration>(true));
        }
        
    }
}