#r "D:\home\site\wwwroot\EpisodeUpdater3\bin\DevPodcasts.DataLayer.dll"
#r "D:\home\site\wwwroot\EpisodeUpdater3\bin\System.ServiceModel.dll"

using DevPodcasts.DataLayer.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

using Microsoft.WindowsAzure.Storage;
using Serilog;
using Serilog.Core;
using System.Configuration;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    const string azureConnection = "DefaultEndpointsProtocol=https;AccountName=devpodcasts;AccountKey=IPP3eNgKtRSxnO6eE3Kjw1VjroOa12KODdwBsoqEMUsvEi+R9RiBf90oP3Ow5g+vVvlg1YqJOj1y1y4wIL64AQ==;EndpointSuffix=core.windows.net";

    var storage = CloudStorageAccount
                .Parse(azureConnection);

    var logger = new LoggerConfiguration()
               .WriteTo.AzureTableStorageWithProperties(storage, storageTableName: "LogProd")
               .MinimumLevel.Debug()
               .CreateLogger();

    const string connection = "Data Source=devpodcasts.database.windows.net;Initial Catalog=devpodcasts;Integrated Security=False;User ID=whiffwhaff9238;Password=mtisaIr2ESthVS64Kx7z;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    var context = new ApplicationDbContext(connection);

    var podcasts = context
                .Podcasts
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .Distinct()
                .ToList();

    int episodesAddedCount = 0;
    var sw = Stopwatch.StartNew();
    logger.Debug($"Begin update of {podcasts.Count} podcasts");
    foreach (var podcast in podcasts)
    {
        var mostRecentEpisodeDate = context
                .Episodes
                .AsNoTracking()
                .Where(i => i.PodcastId == podcast.Id)
                .Select(i => i.DatePublished).Max();


        SyndicationFeed feed = null;
        var feedUrl = podcast.FeedUrl;

        try
        {
            var xmlReader = XmlReader.Create(feedUrl);
            feed = SyndicationFeed.Load(xmlReader);
        }
        catch (Exception ex)
        {
            // Log error
        }

        if (feed != null)
        {
            var newEpisodes = feed
                .Items
                .Where(i => i.PublishDate.DateTime > mostRecentEpisodeDate);

            if (newEpisodes.Count() > 0)
            {
                foreach (var episode in newEpisodes)
                {
                    var e = new Episode
                    {
                        Title = episode.Title.Text,
                        Summary = episode.Summary.Text,
                        AudioUrl = GetAudioUrl(episode),
                        EpisodeUrl = GetEpisodeUrl(episode),
                        DatePublished = episode.PublishDate.DateTime,
                        DateCreated = DateTime.Now
                    };

                    var episodeExistsForPodcast = context.Podcasts
                        .Single(i => i.Id == podcast.Id)
                        .Episodes
                        .Any(i => i.Title == e.Title);

                    if (!episodeExistsForPodcast)
                    {
                        podcast.Episodes.Add(e);
                        episodesAddedCount++;
                        logger.Information(episode.Title + " added");
                    }
                }

                context.SaveChanges();
            }
        }

    }

    if (episodesAddedCount > 0)
    {
        logger.Information($"Number of episodes added: {episodesAddedCount}");
    }
    sw.Stop();
    logger.Debug($"Update took {sw.ElapsedMilliseconds / 1000} seconds");
    log.Info($"Number of episodes added: {episodesAddedCount}");
}

private static string GetEpisodeUrl(SyndicationItem item)
{
    return item.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
}

private static string GetSiteUrl(SyndicationFeed feed)
{
    return feed.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
}

private static string GetAudioUrl(SyndicationItem item)
{
    return item.Links.FirstOrDefault(i => i.RelationshipType == "enclosure")?.Uri.ToString();
}