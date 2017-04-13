namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddEpisodeIdToEpisode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "EpisodeId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episodes", "EpisodeId");
        }
    }
}
