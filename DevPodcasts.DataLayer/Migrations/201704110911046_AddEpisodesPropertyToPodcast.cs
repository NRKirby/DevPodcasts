namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodesPropertyToPodcast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "PodcastId", c => c.Int(nullable: false));
            CreateIndex("dbo.Episodes", "PodcastId");
            AddForeignKey("dbo.Episodes", "PodcastId", "dbo.Podcasts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Episodes", "PodcastId", "dbo.Podcasts");
            DropIndex("dbo.Episodes", new[] { "PodcastId" });
            DropColumn("dbo.Episodes", "PodcastId");
        }
    }
}
