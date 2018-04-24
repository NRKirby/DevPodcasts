using DevPodcasts.Models.Dtos;
using DevPodcasts.Repositories;
using System;
using System.Linq;
using System.ServiceModel.Syndication;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class RssService
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly RssParser _rssParser;

        public RssService(PodcastRepository podcastRepository,
            RssParser rssParser)
        {
            _podcastRepository = podcastRepository;
            _rssParser = rssParser;
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

        private static string GetAudioUrl(SyndicationItem item)
        {
            return item.Links.FirstOrDefault(i => i.RelationshipType == "enclosure")?.Uri.ToString();
        }
    }
}
