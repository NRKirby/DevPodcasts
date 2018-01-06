using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.Logging;
using DevPodcasts.Models;
using DevPodcasts.Repositories;
using System;
using System.Linq;
using System.ServiceModel.Syndication;

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
    }
}
