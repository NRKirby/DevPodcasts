namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddResizedImageUrlToPodcasts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Podcasts", "ResizedImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Podcasts", "ResizedImageUrl");
        }
    }
}
