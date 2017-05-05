using System.ServiceModel.Syndication;

namespace DevPodcasts.ServiceLayer.RSS
{
    public interface IRssParser
    {
        SyndicationFeed ParseRssFeed(string rssFeedUrl);
    }
}
