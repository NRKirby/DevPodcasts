namespace DevPodcasts.DataLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class RemoveSeparateUserEntityAddPropertiesToAspNetUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "User_Id", "dbo.Users");
            DropIndex("dbo.AspNetUsers", new[] { "User_Id" });
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String());
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            DropColumn("dbo.AspNetUsers", "User_Id");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "User_Id", c => c.Int());
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            CreateIndex("dbo.AspNetUsers", "User_Id");
            AddForeignKey("dbo.AspNetUsers", "User_Id", "dbo.Users", "Id");
        }
    }
}
