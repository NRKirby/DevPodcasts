using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer
{
    public class RssParser : IRssParser
    {
        private readonly EpisodeRepository _episodeRepository;
        private readonly PodcastRepository _podcastRepository;

        public RssParser()
        {
            _episodeRepository = new EpisodeRepository();
            _podcastRepository = new PodcastRepository();
        }

        public IEnumerable<EpisodeDto> GetNewEpisodes(PodcastDto podcastDto)
        {
            var mostRecentEpisodeDate = _episodeRepository.GetMostRecentDate(podcastDto.Id);

            if (mostRecentEpisodeDate == null)
                return null;

            var reader = XmlReader.Create(podcastDto.FeedUrl);
            var feed = SyndicationFeed.Load(reader);

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

            var dto = new PodcastDto{ FeedUrl = rssFeedUrl };

            SyndicationFeed feed = null;
            try
            {
                var reader = XmlReader.Create(rssFeedUrl);
                feed = SyndicationFeed.Load(reader);
            }
            catch (Exception ex)
            {
                dto.SuccessResult = SuccessResult.Error;
            }

            if (feed != null)
            {
                if (_podcastRepository.PodcastExists(feed.Title.Text))
                {
                    dto.SuccessResult = SuccessResult.AlreadyExists;
                    return dto;
                }

                var siteUrl = GetSiteUrl(feed);
                dto.Title = feed.Title?.Text;
                dto.Description = feed.Description?.Text;
                dto.ImageUrl = feed.ImageUrl?.AbsoluteUri;
                dto.FeedUrl = rssFeedUrl;
                dto.SiteUrl = siteUrl;
                dto.SuccessResult = SuccessResult.Success;

                return dto;
            }

            return null;
        }

        public void AddPodcastEpisodes(int podcastId)
        {
            var dto = _podcastRepository.GetPodcast(podcastId);

            var reader = XmlReader.Create(dto.FeedUrl);
            var feed = SyndicationFeed.Load(reader);

            var episodes = new List<EpisodeDto>();

            foreach (var item in feed.Items)
            {
                var episodeUrl = item.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
                var audioUrl = item.Links.FirstOrDefault(i => i.RelationshipType == "enclosure")?.Uri.ToString();

                var episode = new EpisodeDto
                {
                    Title = item.Title?.Text,
                    Summary = item.Summary?.Text,
                    EpisodeUrl = episodeUrl,
                    AudioUrl = audioUrl,
                    DatePublished = item.PublishDate.DateTime,
                    DateCreated = DateTime.Now
                };
                episodes.Add(episode);
            }

            dto.Episodes = episodes;

            _podcastRepository.AddEpisodesToPodcast(dto);
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
