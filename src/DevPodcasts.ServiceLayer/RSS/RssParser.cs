using DevPodcasts.Logging;
using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using DevPodcasts.Models;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class RssParser
    {
        private readonly ILogger _logger;

        public RssParser(ILogger logger)
        {
            _logger = logger;
        }

        public RssFeed ParseRssFeed(string rssFeedUrl)
        {
            SyndicationFeed feed;
            try
            {
                var xmlReader = XmlReader.Create(rssFeedUrl);
                feed = SyndicationFeed.Load(xmlReader);
            }
            catch (Exception ex)
            {
                _logger.Error(rssFeedUrl, ex);
                throw;
            }

            var episodeSummaryLocation = GetEpisodeSummaryLocation(feed);

            var rssFeed = new RssFeed
            {
                SyndicationFeed = feed,
                EpisodeSummaryLocation = episodeSummaryLocation
            };

            return rssFeed;
        }

        private EpisodeSummaryLocation GetEpisodeSummaryLocation(SyndicationFeed feed)
        {
            if (feed.Items.Any(episode => episode.Summary != null))
            {
                return EpisodeSummaryLocation.Summmary;
            }
            if (feed.Items.Any(episode => episode.Content != null))
            {
                return EpisodeSummaryLocation.Content;
            }

            return EpisodeSummaryLocation.NotSet;
        }
    }
}
