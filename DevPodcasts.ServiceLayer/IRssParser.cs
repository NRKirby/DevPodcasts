using System.ServiceModel.Syndication;

namespace DevPodcasts.ServiceLayer
{
    public interface IRssParser
    {
        SyndicationFeed ParseRssFeed(string rssFeedUrl);
    }
}
