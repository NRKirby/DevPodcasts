namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodeUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Episodes", "EpisodeUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Episodes", "EpisodeUrl");
        }
    }
}
