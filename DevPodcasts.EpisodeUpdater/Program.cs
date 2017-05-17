using System.Web.Mvc;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Logging;
using DevPodcasts.ServiceLayer.RSS;

namespace DevPodcasts.EpisodeUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString =
                "DefaultEndpointsProtocol=https;AccountName=devpodcasts;AccountKey=IPP3eNgKtRSxnO6eE3Kjw1VjroOa12KODdwBsoqEMUsvEi+R9RiBf90oP3Ow5g+vVvlg1YqJOj1y1y4wIL64AQ==;EndpointSuffix=core.windows.net";

            var updater = new EpisodeUpdater(
                new PodcastRepository(), 
                new EpisodeRepository(), 
                new RssService(new EpisodeRepository(), new PodcastRepository(), new RssParser(new AzureTableLogger(connectionString)), new AzureTableLogger(connectionString)), 
                new AzureTableLogger(connectionString));

            updater.UpdateSync();
        }
    }
}
