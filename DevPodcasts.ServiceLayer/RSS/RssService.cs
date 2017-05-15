using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class RssService
    {
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IPodcastRepository _podcastRepository;
        private readonly RssParser _rssParser;
        private readonly ILogger _logger;

        public RssService(IEpisodeRepository episodeRepository,
            IPodcastRepository podcastRepository,
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

            var episodes = new List<EpisodeDto>();

            if (feed == null)
                return null;

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
            //if (!IsValidUrl(model.RssFeedUrl)) // TODO NK - returning false for https://fivejs.codeschool.com/feed.rss
            //{
            //    podcastDto.SuccessResult = SuccessResult.InvalidUrl;
            //    return podcastDto;
            //}

            var dto = new PodcastDto
            {
                FeedUrl = rssFeedUrl,
                SuccessResult = SuccessResult.NotSet
            };

            if (_podcastRepository.PodcastExists(rssFeedUrl))
            {
                dto.SuccessResult = SuccessResult.AlreadyExists;
                _logger.Error(rssFeedUrl + " : Already exists", null);
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
                // Error should be logged in RssParser
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

        // TODO
        private static bool IsValidUrl(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }
    }
}
