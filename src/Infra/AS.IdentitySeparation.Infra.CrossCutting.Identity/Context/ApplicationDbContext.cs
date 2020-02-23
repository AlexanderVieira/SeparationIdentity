using AS.IdentitySeparation.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace AS.IdentitySeparation.Infra.CrossCutting.Identity.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDisposable
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}