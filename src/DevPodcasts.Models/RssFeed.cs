using System.ServiceModel.Syndication;

namespace DevPodcasts.Models
{
    public class RssFeed
    {
        public SyndicationFeed SyndicationFeed { get; set; }

        public EpisodeSummaryLocation EpisodeSummaryLocation { get; set; }
    }
}
