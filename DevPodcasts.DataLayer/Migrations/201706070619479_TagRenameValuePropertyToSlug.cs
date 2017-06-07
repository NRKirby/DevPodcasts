namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagRenameValuePropertyToSlug : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Slug", c => c.String());
            DropColumn("dbo.Tags", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tags", "Value", c => c.String());
            DropColumn("dbo.Tags", "Slug");
        }
    }
}
