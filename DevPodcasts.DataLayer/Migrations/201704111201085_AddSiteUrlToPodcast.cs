namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddSiteUrlToPodcast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Podcasts", "SiteUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Podcasts", "SiteUrl");
        }
    }
}
