using System;
using System.Threading;
using DevPodcasts.Logging;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.RSS;

namespace DevPodcasts.EpisodeUpdater
{
    class Program
    {
        private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=devpodcasts;AccountKey=IPP3eNgKtRSxnO6eE3Kjw1VjroOa12KODdwBsoqEMUsvEi+R9RiBf90oP3Ow5g+vVvlg1YqJOj1y1y4wIL64AQ==;EndpointSuffix=core.windows.net";
        private const string TableName = "LogProd";

        static void Main(string[] args)
        {
            var updater = new EpisodeUpdater(
                new PodcastRepository(),
                new EpisodeRepository(new AzureTableLogger(ConnectionString, TableName)),
                new RssService(new EpisodeRepository(new AzureTableLogger(ConnectionString, TableName)), new PodcastRepository(), new RssParser(new AzureTableLogger(ConnectionString, TableName)), new AzureTableLogger(ConnectionString, TableName)),
                new AzureTableLogger(ConnectionString, TableName));

            while (true)
            {
                Console.WriteLine("Begin update: " + DateTime.Now);
                updater.UpdateSync();
                Console.WriteLine("Finish update: " + DateTime.Now);
                const int minutes = 60;
                Thread.Sleep(1000 * 60 * minutes); // 1000ms * 60s * minutes
            }
        }
    }
}
