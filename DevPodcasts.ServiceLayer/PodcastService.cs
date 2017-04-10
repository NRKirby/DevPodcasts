using DevPodcasts.ViewModels.Podcast;
using System;
using System.Xml;
using System.ServiceModel.Syndication;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService
    {
        public AddPodcastViewModel Add(AddPodcastViewModel model)
        {
            if (!IsValidUrl(model.RssFeedUrl))
            {
                model.Result = SuccessResult.InvalidUrl;
                return model;
            }

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

            if (!IsEnglishLanguage(feed))
            {
                model.Result = SuccessResult.NotEnglish;
                return model;
            }

            if (feed != null)
            {
                // Add to podcast review queue
                model.Result = SuccessResult.Success;
            }

            return model;
        }

        private static bool IsValidUrl(string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }

        private bool IsEnglishLanguage(SyndicationFeed feed)
        {
            return feed.Language.ToLower().Contains("en");
        }
    }
}
