namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateLibraryPodcastTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LibraryPodcasts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PodcastId = c.Int(nullable: false),
                        UserId = c.String(),
                        IsSubscribed = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Podcasts", t => t.PodcastId, cascadeDelete: true)
                .Index(t => t.PodcastId)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LibraryPodcasts", "PodcastId", "dbo.Podcasts");
            DropForeignKey("dbo.LibraryPodcasts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.LibraryPodcasts", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.LibraryPodcasts", new[] { "PodcastId" });
            DropTable("dbo.LibraryPodcasts");
        }
    }
}
