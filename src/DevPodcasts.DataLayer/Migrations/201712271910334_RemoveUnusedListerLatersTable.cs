namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnusedListerLatersTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ListenLaters", "EpisodeId", "dbo.Episodes");
            DropIndex("dbo.ListenLaters", new[] { "EpisodeId" });
            DropTable("dbo.ListenLaters");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ListenLaters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        EpisodeId = c.Int(nullable: false),
                        AddedTimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ListenLaters", "EpisodeId");
            AddForeignKey("dbo.ListenLaters", "EpisodeId", "dbo.Episodes", "Id", cascadeDelete: true);
        }
    }
}
