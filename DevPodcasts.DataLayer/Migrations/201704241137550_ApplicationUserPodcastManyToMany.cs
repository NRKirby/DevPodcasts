namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ApplicationUserPodcastManyToMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPodcast",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        PodcastId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PodcastId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Podcasts", t => t.PodcastId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PodcastId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPodcast", "PodcastId", "dbo.Podcasts");
            DropForeignKey("dbo.UserPodcast", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserPodcast", new[] { "PodcastId" });
            DropIndex("dbo.UserPodcast", new[] { "UserId" });
            DropTable("dbo.UserPodcast");
        }
    }
}
