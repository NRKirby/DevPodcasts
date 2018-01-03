namespace DevPodcasts.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPodcastTitleToLibraryPodcast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LibraryPodcasts", "PodcastTitle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LibraryPodcasts", "PodcastTitle");
        }
    }
}
