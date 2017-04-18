using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer
{
    public class RssParser
    {
        public SyndicationFeed ParseRssFeed(string rssFeedUrl)
        {
            var xmlReader = XmlReader.Create(rssFeedUrl);
            var feed = SyndicationFeed.Load(xmlReader);

            return feed;
        }
    }
}
