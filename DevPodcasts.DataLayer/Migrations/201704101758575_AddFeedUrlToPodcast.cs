namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddFeedUrlToPodcast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Podcasts", "FeedUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Podcasts", "FeedUrl");
        }
    }
}
