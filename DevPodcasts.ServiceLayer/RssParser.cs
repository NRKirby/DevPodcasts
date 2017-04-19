﻿using DevPodcasts.ServiceLayer.Logging;
using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer
{
    public class RssParser : IRssParser
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
                _logger.Error(rssFeedUrl, ex);
                throw;
            }
            

            return feed;
        }
    }
}
