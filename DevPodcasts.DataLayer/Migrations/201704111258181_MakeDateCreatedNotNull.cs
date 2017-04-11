namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeDateCreatedNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Podcasts", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Podcasts", "DateCreated", c => c.DateTime());
        }
    }
}
