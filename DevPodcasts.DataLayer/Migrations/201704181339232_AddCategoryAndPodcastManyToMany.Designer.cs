// <auto-generated />
namespace DevPodcasts.DataLayer.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class AddCategoryAndPodcastManyToMany : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddCategoryAndPodcastManyToMany));
        
        string IMigrationMetadata.Id
        {
            get { return "201704181339232_AddCategoryAndPodcastManyToMany"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}