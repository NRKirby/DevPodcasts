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

        public ApplicationDbContext(string connection)
            : base(connection, throwIfV1Schema: false)
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

            modelBuilder.Entity<Podcast>()
                .HasMany(p => p.Tags)
                .WithMany(c => c.Podcasts)
                .Map(pc =>
                {
                    pc.MapLeftKey("PodcastId");
                    pc.MapRightKey("TagId");
                    pc.ToTable("PodcastTag");
                });

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(user => user.SubscribedPodcasts)
                .WithMany(podcast => podcast.SubscribedUsers)
                .Map(userPodcast =>
                {
                    userPodcast.MapLeftKey("UserId");
                    userPodcast.MapRightKey("PodcastId");
                    userPodcast.ToTable("UserPodcast");
                });
        }

        public DbSet<Podcast> Podcasts { get; set; }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ListenLater> ListenLaters { get; set; }

        public DbSet<LibraryEpisode> LibraryEpisodes { get; set; }
    }
}