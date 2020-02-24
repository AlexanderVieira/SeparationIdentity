using System.Data.Entity;

namespace AS.IdentitySeparation.Infra.Data.Sqlite.Context
{
    public class IdentitySeparationDbContext : DbContext
    {
        public IdentitySeparationDbContext() : base("ConnStrIdentity")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}