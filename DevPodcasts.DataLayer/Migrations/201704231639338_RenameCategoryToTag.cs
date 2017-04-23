namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RenameCategoryToTag : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PodcastCategory", "PodcastId", "dbo.Podcasts");
            DropForeignKey("dbo.PodcastCategory", "CategoryId", "dbo.Categories");
            DropIndex("dbo.PodcastCategory", new[] { "PodcastId" });
            DropIndex("dbo.PodcastCategory", new[] { "CategoryId" });
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.PodcastTag",
                c => new
                    {
                        PodcastId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PodcastId, t.TagId })
                .ForeignKey("dbo.Podcasts", t => t.PodcastId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.TagId, cascadeDelete: true)
                .Index(t => t.PodcastId)
                .Index(t => t.TagId);
            
            DropTable("dbo.Categories");
            DropTable("dbo.PodcastCategory");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PodcastCategory",
                c => new
                    {
                        PodcastId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PodcastId, t.CategoryId });
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            DropForeignKey("dbo.PodcastTag", "TagId", "dbo.Tags");
            DropForeignKey("dbo.PodcastTag", "PodcastId", "dbo.Podcasts");
            DropIndex("dbo.PodcastTag", new[] { "TagId" });
            DropIndex("dbo.PodcastTag", new[] { "PodcastId" });
            DropTable("dbo.PodcastTag");
            DropTable("dbo.Tags");
            CreateIndex("dbo.PodcastCategory", "CategoryId");
            CreateIndex("dbo.PodcastCategory", "PodcastId");
            AddForeignKey("dbo.PodcastCategory", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            AddForeignKey("dbo.PodcastCategory", "PodcastId", "dbo.Podcasts", "Id", cascadeDelete: true);
        }
    }
}
