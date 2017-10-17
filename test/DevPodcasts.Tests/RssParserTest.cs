using DevPodcasts.Logging;
using DevPodcasts.Models;
using DevPodcasts.ServiceLayer.RSS;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevPodcasts.Tests
{
    [TestClass]
    public class RssParserTest
    {
        [TestMethod]
        public void DoubleYourFreelancingPodcast_EpisodeSummaryLocationShouldBeContent()
        {
            var fakeLogger = A.Fake<ILogger>();
            var sut = new RssParser(fakeLogger);
            const string feedUrl = "https://rss.simplecast.com/podcasts/219/rss";

            var result = sut.ParseRssFeed(feedUrl);

            Assert.AreEqual(EpisodeSummaryLocation.Content, result.EpisodeSummaryLocation);
        }
    }
}
