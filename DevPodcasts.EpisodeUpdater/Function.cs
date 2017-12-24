using DevPodcasts.DataLayer.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.EpisodeUpdater
{
    public static class Function
    {
        [FunctionName("Function")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            const string connectionString = "Server=tcp:devpodcasts.database.windows.net,1433;Initial Catalog=devpodcasts;Persist Security Info=False;User ID=whiffwhaff9238;Password=mtisaIr2ESthVS64Kx7z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var context = new ApplicationDbContext(connectionString);

            var podcasts = context.Podcasts
                .Where(p => p.IsApproved == true)
                .OrderBy(p => p.Title)
                .Distinct()
                .ToList();

            var episodesAddedCount = 0;

            foreach (var podcast in podcasts)
            {
                var mostRecentEpisodeDate = GetMostRecentEpisodeDate(podcast);

                SyndicationFeed feed = null;
                var feedUrl = podcast.FeedUrl;

                try
                {
                    var xmlReader = XmlReader.Create(feedUrl);
                    feed = SyndicationFeed.Load(xmlReader);
                }
                catch (Exception ex)
                {
                    MethodBase site = ex.TargetSite;
                    string methodName = site?.Name;

                    log.Error($"{podcast.Title} - Method: {methodName} Message: {ex.Message}", ex);
                }
                if (feed != null)
                {
                    var newEpisodes = feed
                        .Items
                        .Where(i => i.PublishDate.DateTime > mostRecentEpisodeDate);

                    if (newEpisodes.Any())
                    {
                        try
                        {
                            var podcastToAddEpisodesTo = context.Podcasts.Single(p => p.Id == podcast.Id);

                            foreach (var newEpisode in newEpisodes)
                            {
                                var episode = new Episode
                                {
                                    Title = newEpisode.Title?.Text,
                                    Summary = newEpisode.Summary?.Text,
                                    AudioUrl = GetAudioUrl(newEpisode),
                                    EpisodeUrl = GetEpisodeUrl(newEpisode),
                                    DatePublished = newEpisode.PublishDate.DateTime,
                                    DateCreated = DateTime.Now
                                };

                                var episodeExistsForPodcast = context.Podcasts
                                    .Single(i => i.Id == podcast.Id)
                                    .Episodes
                                    .Any(i => i.Title == episode.Title);

                                if (!episodeExistsForPodcast)
                                {
                                    podcastToAddEpisodesTo.Episodes.Add(episode);
                                    episodesAddedCount++;
                                    log.Info($"{podcast.Title} {episode.Title} added");
                                    context.SaveChanges();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            MethodBase site = ex.TargetSite;
                            string methodName = site?.Name;

                            log.Error($"{podcast.Title} - Method: {methodName} Message: {ex.Message}", ex);
                        }
                    }
                }

                if (episodesAddedCount > 0)
                {
                    log.Info($"Number of episodes added: {episodesAddedCount}");
                }
            }
        }

        private static DateTime? GetMostRecentEpisodeDate(Podcast podcast)
        {
            const string connectionString = "Server=tcp:devpodcasts.database.windows.net,1433;Initial Catalog=devpodcasts;Persist Security Info=False;User ID=whiffwhaff9238;Password=mtisaIr2ESthVS64Kx7z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var context = new ApplicationDbContext(connectionString);

            return context.Episodes
                .AsNoTracking()
                .Where(i => i.PodcastId == podcast.Id)
                .Select(i => i.DatePublished)
                .Max();
        }

        private static string GetEpisodeUrl(SyndicationItem item)
        {
            return item.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
        }

        private static string GetAudioUrl(SyndicationItem item)
        {
            return item.Links.FirstOrDefault(i => i.RelationshipType == "enclosure")?.Uri.ToString();
        }
    }
}

