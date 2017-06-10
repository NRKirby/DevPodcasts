namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddValuePropertyToTagEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tags", "Value", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tags", "Value");
        }
    }
}
