using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService
    {
        private readonly PodcastRepository _repository;
        private readonly RssParser _parser;

        public PodcastService()
        {
            _repository = new PodcastRepository();
            _parser = new RssParser();
        }

        public async Task<AddPodcastViewModel> AddPodcastForReview(AddPodcastViewModel model)
        {
            var podcastDto = _parser.GetPodcastForReview(model.RssFeedUrl);
            if (podcastDto.SuccessResult == SuccessResult.Success)
            {
                await _repository.Add(podcastDto);
            }

            var viewModel = new AddPodcastViewModel
            {
                RssFeedUrl = podcastDto.FeedUrl,
                SuccessResult = podcastDto.SuccessResult
            };

            return viewModel;
        }
        
        public void AddPodcastEpisodes(int podcastId)
        {
            var dto = _repository.GetPodcast(podcastId);

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

            _repository.AddEpisodesToPodcast(dto);
        }
    }
}
