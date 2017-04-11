using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Podcast;
using System;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService
    {
        private PodcastRepository _repository;

        public PodcastService()
        {
            _repository = new PodcastRepository();
        }

        public async Task<AddPodcastViewModel> Add(AddPodcastViewModel model)
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
            catch (Exception)
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
                await AddPodcast(feed, model.RssFeedUrl);
            }

            return model;
        }

        private static bool IsValidUrl(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }

        private async Task AddPodcast(SyndicationFeed feed, string rssFeedUrl)
        {
            var dto = new PodcastDto
            {
                Title = feed.Title?.Text,
                Description = feed.Description?.Text,
                ImageUrl = feed.ImageUrl?.AbsoluteUri,
                FeedUrl = rssFeedUrl,
                SiteUrl = feed.Links[1].Uri.ToString()
            };
            await _repository.Add(dto);
        }
    }
}
