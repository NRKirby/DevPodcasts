using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.Logging;
using DevPodcasts.Models;
using DevPodcasts.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class RssService
    {
        private readonly EpisodeRepository _episodeRepository;
        private readonly PodcastRepository _podcastRepository;
        private readonly RssParser _rssParser;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;

        public RssService(EpisodeRepository episodeRepository,
            PodcastRepository podcastRepository,
            RssParser rssParser,
            ILogger logger,
            ApplicationDbContext context)
        {
            _episodeRepository = episodeRepository;
            _podcastRepository = podcastRepository;
            _rssParser = rssParser;
            _logger = logger;
            _context = context;
        }

        public IEnumerable<EpisodeDto> GetNewEpisodes(PodcastDto podcastDto)
        {
            var mostRecentEpisodeDate = _episodeRepository.GetMostRecentEpisodeDate(podcastDto.Id);

            if (mostRecentEpisodeDate == null)
                return null;

            var feed = _rssParser.ParseRssFeed(podcastDto.FeedUrl);

            if (feed == null)
                return null;

            var episodes = new List<EpisodeDto>();

            var newEpisodes = feed.SyndicationFeed.Items
                .Where(i => i.PublishDate.DateTime > mostRecentEpisodeDate);

            foreach (var newEpisode in newEpisodes)
            {
                var episodeUrl = GetSiteUrl(feed.SyndicationFeed);
                var audioUrl = GetAudioUrl(newEpisode);

                var episode = new EpisodeDto
                {
                    EpisodeId = newEpisode.Id,
                    Title = newEpisode.Title?.Text,
                    Summary = newEpisode.Summary?.Text,
                    EpisodeUrl = episodeUrl,
                    AudioUrl = audioUrl,
                    DatePublished = newEpisode.PublishDate.DateTime,
                    DateCreated = DateTime.Now,
                    PodcastId = podcastDto.Id
                };
                episodes.Add(episode);
            }

            return episodes;
        }

        public PodcastDto GetPodcastForReview(string rssFeedUrl)
        {
            var dto = new PodcastDto
            {
                FeedUrl = rssFeedUrl
            };

            if (!IsValidUrl(rssFeedUrl))
            {
                dto.SuccessResult = SuccessResult.InvalidUrl;
                _logger.Error(rssFeedUrl + " : Invalid URL", null);
                return dto;
            }

            if (_podcastRepository.PodcastExists(rssFeedUrl))
            {
                dto.SuccessResult = SuccessResult.AlreadyExists;
                _logger.Info(rssFeedUrl + " :: Podcast feed already exists");
                return dto;
            }

            RssFeed feed = new RssFeed();
            try
            {
                feed = _rssParser.ParseRssFeed(rssFeedUrl);
            }
            catch (Exception)
            {
                dto.SuccessResult = SuccessResult.Error;
            }

            if (feed == null) return dto;
            var siteUrl = GetSiteUrl(feed.SyndicationFeed);
            dto.Title = feed.SyndicationFeed.Title?.Text;
            dto.Description = feed.SyndicationFeed.Description?.Text;
            dto.ImageUrl = feed.SyndicationFeed.ImageUrl?.AbsoluteUri;
            dto.FeedUrl = rssFeedUrl;
            dto.SiteUrl = siteUrl;
            dto.SuccessResult = SuccessResult.Success;

            return dto;
        }

        public void AddPodcastEpisodes(int podcastId)
        {
            var dto = _podcastRepository.GetPodcast(podcastId);

            var feed = _rssParser.ParseRssFeed(dto.FeedUrl);

            if (feed == null)
                return;

            var episodes = from item in feed.SyndicationFeed.Items
                let episodeUrl = GetEpisodeUrl(item)
                let audioUrl = GetAudioUrl(item)
                select new EpisodeDto
                {
                    Title = item.Title?.Text,
                    Summary = item.Summary?.Text,
                    EpisodeUrl = episodeUrl,
                    AudioUrl = audioUrl,
                    DatePublished = item.PublishDate.DateTime,
                    DateCreated = DateTime.Now
                };

            dto.Episodes = episodes;

            _podcastRepository.AddEpisodesToPodcast(dto);
        }

        public void UpdateEpisodes()
        {
            var podcasts = _context
                .Podcasts
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .Distinct()
                .ToList();

            int episodesAddedCount = 0;
            var sw = Stopwatch.StartNew();
            _logger.Debug($"Begin update of {podcasts.Count} podcasts");

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

                    _logger.Error($"{podcast.Title} - Method: {methodName} Message: {ex.Message}", ex);
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
                            var podcastToAddEpisodesTo = _context.Podcasts.Single(p => p.Id == podcast.Id);

                            foreach (var episode in newEpisodes)
                            {
                                var e = new DataLayer.Models.Episode
                                {
                                    Title = episode.Title?.Text,
                                    Summary = episode.Summary?.Text,
                                    AudioUrl = GetAudioUrl(episode),
                                    EpisodeUrl = GetEpisodeUrl(episode),
                                    DatePublished = episode.PublishDate.DateTime,
                                    DateCreated = DateTime.Now
                                };

                                var episodeExistsForPodcast = _context.Podcasts
                                    .Single(i => i.Id == podcast.Id)
                                    .Episodes
                                    .Any(i => i.Title == e.Title);

                                if (!episodeExistsForPodcast)
                                {
                                    podcastToAddEpisodesTo.Episodes.Add(e);
                                    episodesAddedCount++;
                                    _logger.Info($"{podcast.Title} {e.Title} added");
                                    _context.SaveChanges();
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            MethodBase site = ex.TargetSite;
                            string methodName = site?.Name;

                            _logger.Error($"{podcast.Title} - Method: {methodName} Message: {ex.Message}", ex);
                        }
                    }
                }

                if (episodesAddedCount > 0)
                {
                    _logger.Info($"Number of episodes added: {episodesAddedCount}");
                }

                sw.Stop();
                _logger.Debug($"Update took {sw.ElapsedMilliseconds / 1000} seconds");
            }
        }

        private static string GetEpisodeUrl(SyndicationItem item)
        {
            return item.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
        }

        private string GetSiteUrl(SyndicationFeed feed)
        {
            return feed.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
        }

        private string GetAudioUrl(SyndicationItem item)
        {
            return item.Links.FirstOrDefault(i => i.RelationshipType == "enclosure")?.Uri.ToString();
        }

        private static bool IsValidUrl(string url)
        {
            bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        private DateTime? GetMostRecentEpisodeDate(DataLayer.Models.Podcast podcast)
        {
            return _context.Episodes
                .AsNoTracking()
                .Where(i => i.PodcastId == podcast.Id)
                .Select(i => i.DatePublished)
                .Max();
        }
    }
}
