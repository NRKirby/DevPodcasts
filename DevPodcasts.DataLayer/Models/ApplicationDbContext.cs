using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DevPodcasts.DataLayer.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var conventions = new List<PluralizingTableNameConvention>().ToArray();
            modelBuilder.Conventions.Remove(conventions);
        }

        public DbSet<Podcast> Podcasts { get; set; }

        public DbSet<Episode> Episodes { get; set; }
    }
}