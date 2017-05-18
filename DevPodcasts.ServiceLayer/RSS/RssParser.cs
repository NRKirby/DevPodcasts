using DevPodcasts.Logging;
using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class RssParser
    {
        private readonly ILogger _logger;

        public RssParser(ILogger logger)
        {
            _logger = logger;
        }

        public SyndicationFeed ParseRssFeed(string rssFeedUrl)
        {
            SyndicationFeed feed;
            try
            {
                var xmlReader = XmlReader.Create(rssFeedUrl);
                feed = SyndicationFeed.Load(xmlReader);
            }
            catch (Exception ex)
            {
                feed = null;
                _logger.Error(rssFeedUrl, ex);
            }

            return feed;
        }
    }
}
