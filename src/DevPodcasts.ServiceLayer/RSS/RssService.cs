using DevPodcasts.Dtos;
using DevPodcasts.Logging;
using DevPodcasts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using DevPodcasts.Models;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class RssService
    {
        private readonly EpisodeRepository _episodeRepository;
        private readonly PodcastRepository _podcastRepository;
        private readonly RssParser _rssParser;
        private readonly ILogger _logger;

        public RssService(EpisodeRepository episodeRepository,
            PodcastRepository podcastRepository,
            RssParser rssParser,
            ILogger logger)
        {
            _episodeRepository = episodeRepository;
            _podcastRepository = podcastRepository;
            _rssParser = rssParser;
            _logger = logger;
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

            var newEpisodes = feed.Items
                .Where(i => i.PublishDate.DateTime > mostRecentEpisodeDate);

            foreach (var newEpisode in newEpisodes)
            {
                var episodeUrl = GetSiteUrl(feed);
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

            SyndicationFeed feed = null;
            try
            {
                feed = _rssParser.ParseRssFeed(rssFeedUrl);
            }
            catch (Exception)
            {
                dto.SuccessResult = SuccessResult.Error;
            }

            if (feed == null) return dto;
            var siteUrl = GetSiteUrl(feed);
            dto.Title = feed.Title?.Text;
            dto.Description = feed.Description?.Text;
            dto.ImageUrl = feed.ImageUrl?.AbsoluteUri;
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

            var episodes = from item in feed.Items
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
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }
    }
}
