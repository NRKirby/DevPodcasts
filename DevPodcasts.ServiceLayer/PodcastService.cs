using System;
using System.Xml;
using System.ServiceModel.Syndication;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService
    {
        public bool Add(string url)
        {
            SyndicationFeed feed;
            try
            {
                var reader = XmlReader.Create(url);
                feed = SyndicationFeed.Load(reader);
            }
            catch (Exception)
            {
                return false;
            }

            if (feed != null)
            {
                // Add to podcast review queue
                return true;
            }

            return false;
        }
    }
}
