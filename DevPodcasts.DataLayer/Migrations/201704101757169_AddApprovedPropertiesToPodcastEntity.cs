namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddApprovedPropertiesToPodcastEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Podcasts", "IsApproved", c => c.Boolean());
            AddColumn("dbo.Podcasts", "DateApproved", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Podcasts", "DateApproved");
            DropColumn("dbo.Podcasts", "IsApproved");
        }
    }
}
