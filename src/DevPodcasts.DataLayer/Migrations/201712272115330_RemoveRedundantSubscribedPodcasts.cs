namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRedundantSubscribedPodcasts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserPodcast", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserPodcast", "PodcastId", "dbo.Podcasts");
            DropIndex("dbo.UserPodcast", new[] { "UserId" });
            DropIndex("dbo.UserPodcast", new[] { "PodcastId" });
            DropTable("dbo.UserPodcast");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserPodcast",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        PodcastId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PodcastId });
            
            CreateIndex("dbo.UserPodcast", "PodcastId");
            CreateIndex("dbo.UserPodcast", "UserId");
            AddForeignKey("dbo.UserPodcast", "PodcastId", "dbo.Podcasts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserPodcast", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
