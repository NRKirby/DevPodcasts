using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer
{
    public class RssParser
    {
        private readonly EpisodeRepository _episodeRepository;

        public RssParser()
        {
            _episodeRepository = new EpisodeRepository();
        }

        public IEnumerable<EpisodeDto> GetNewEpisodes(PodcastDto dto)
        {
            var mostRecentEpisodeDate = _episodeRepository.GetMostRecentDate(dto.Id);

            if (mostRecentEpisodeDate == null)
                return null;

            var reader = XmlReader.Create(dto.FeedUrl);
            var feed = SyndicationFeed.Load(reader);

            var episodes = new List<EpisodeDto>();

            if (feed == null)
                return null;

            var newEpisodes = feed.Items
                .Where(i => i.PublishDate.DateTime > mostRecentEpisodeDate);

            foreach (var newEpisode in newEpisodes)
            {
                var episodeUrl = newEpisode.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
                var audioUrl = newEpisode.Links.FirstOrDefault(i => i.RelationshipType == "enclosure")?.Uri.ToString();

                var episode = new EpisodeDto
                {
                    EpisodeId = newEpisode.Id,
                    Title = newEpisode.Title?.Text,
                    Summary = newEpisode.Summary?.Text,
                    EpisodeUrl = episodeUrl,
                    AudioUrl = audioUrl,
                    DatePublished = newEpisode.PublishDate.DateTime,
                    DateCreated = DateTime.Now,
                    PodcastId = dto.Id
                };
                episodes.Add(episode);
            }

            return episodes;
        }
    }
}
