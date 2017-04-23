namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddCategoryAndPodcastManyToMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.PodcastCategory",
                c => new
                    {
                        PodcastId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PodcastId, t.CategoryId })
                .ForeignKey("dbo.Podcasts", t => t.PodcastId, cascadeDelete: true)
                .ForeignKey("dbo.Tags", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.PodcastId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PodcastCategory", "TagId", "dbo.Tags");
            DropForeignKey("dbo.PodcastCategory", "PodcastId", "dbo.Podcasts");
            DropIndex("dbo.PodcastCategory", new[] { "TagId" });
            DropIndex("dbo.PodcastCategory", new[] { "PodcastId" });
            DropTable("dbo.PodcastCategory");
            DropTable("dbo.Tags");
        }
    }
}
