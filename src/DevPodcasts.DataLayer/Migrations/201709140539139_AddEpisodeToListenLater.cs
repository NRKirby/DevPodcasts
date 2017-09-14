namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEpisodeToListenLater : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ListenLaters", "EpisodeId");
            AddForeignKey("dbo.ListenLaters", "EpisodeId", "dbo.Episodes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListenLaters", "EpisodeId", "dbo.Episodes");
            DropIndex("dbo.ListenLaters", new[] { "EpisodeId" });
        }
    }
}
