using System;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace DevPodcasts.ServiceLayer
{
    public class RssParser
    {
        public SyndicationFeed ParseRssFeed(string rssFeedUrl)
        {
            //string xml; TODO (http://developeronfire.com/rss.xml)
            //using (WebClient webClient = new WebClient())
            //{
            //    xml = Encoding.UTF8.GetString(webClient.DownloadData(rssFeedUrl));
            //}
            //xml = xml.Replace("2017-04-13", "Thu, 13 Apr 2017");
            //byte[] bytes = Encoding.ASCII.GetBytes(xml);
            //XmlReader reader = XmlReader.Create(new MemoryStream(bytes));
            //SyndicationFeed feed2 = SyndicationFeed.Load(reader);


            var xmlReader = XmlReader.Create(rssFeedUrl);
            var feed = SyndicationFeed.Load(xmlReader);

            return feed;
        }
    }
}
