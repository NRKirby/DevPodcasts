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

        public PodcastService()
        {
            _repository = new PodcastRepository();
        }

        public async Task<AddPodcastViewModel> AddPodcastForReview(AddPodcastViewModel model)
        {
            //if (!IsValidUrl(model.RssFeedUrl)) // TODO NK - returning false for https://fivejs.codeschool.com/feed.rss
            //{
            //    model.Result = SuccessResult.InvalidUrl;
            //    return model;
            //}

            SyndicationFeed feed = null;
            try
            {
                var reader = XmlReader.Create(model.RssFeedUrl);
                feed = SyndicationFeed.Load(reader);
            }
            catch (Exception ex)
            {
                model.Result = SuccessResult.Error;
            }

            if (feed != null)
            {
                if (_repository.PodcastExists(feed.Title.Text))
                {
                    model.Result = SuccessResult.AlreadyExists;
                    return model;
                }

                model.Result = SuccessResult.Success;
                await AddPodcastForReview(feed, model.RssFeedUrl);
            }

            return model;
        }

        private static bool IsValidUrl(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }

        private async Task AddPodcastForReview(SyndicationFeed feed, string rssFeedUrl)
        {
            var siteUrl = feed.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
            var dto = new PodcastDto
            {
                Title = feed.Title?.Text,
                Description = feed.Description?.Text,
                ImageUrl = feed.ImageUrl?.AbsoluteUri,
                FeedUrl = rssFeedUrl,
                SiteUrl = siteUrl
            };
            await _repository.Add(dto);
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
