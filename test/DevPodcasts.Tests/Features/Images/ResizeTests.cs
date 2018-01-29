using DevPodcasts.Web.Features.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevPodcasts.Tests.Features.Images
{
    [TestClass]
    public class ResizeTests : TestBase
    {
        [TestMethod]
        public void Resize_AgileInThreeMinutes_ShouldReturnResizedImage()
        {
            const int width = 500;
            const int height = 500;
            const string url = "https://agilein3minut.es/images/ai3m.png";
            const string podcastTitle = "Agile in 3 minutes";

            var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle}).Result;

            Assert.AreEqual("agile_in_3_minutes.png", resizedImage.ImageName);
        }

        [TestMethod]
        public void Resize_NinetyNinePercentInvisible_ShouldReturnResizedImage()
        {
            const int width = 500;
            const int height = 500;
            const string url = "https://f.prxu.org/99pi/images/42384e27-3dd6-497f-991f-67fabb7e6e5b/99-300.png";
            const string podcastTitle = "99% Invisible";

            var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle }).Result;

            Assert.AreEqual("99_invisible.png", resizedImage.ImageName);
        }

        [TestMethod]
        public void Resize_LetsMakeMistakes_ShouldReturnResizedImage()
        {
            const int width = 500;
            const int height = 500;
            const string url = "http://i1.sndcdn.com/avatars-000180812387-au235m-original.jpg";
            const string podcastTitle = "Let\'s Make Mistakes";

            var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle }).Result;

            Assert.AreEqual("lets_make_mistakes.jpeg", resizedImage.ImageName);
        }

        [TestMethod]
        public void Resize_InitPython_ShouldReturnNullAsSmallerThanFiveHundredPixels()
        {
            const int width = 500;
            const int height = 500;
            const string url = "https://www.podcastinit.com/wp-content/cache/podlove/7a/cf0997aacd0c64b37245739c9c9085/podcast-__init__python_original.png";
            const string podcastTitle = "Podcast.__init__(\'Python\')";

            var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle }).Result;

            Assert.IsNull(resizedImage);
        }

        [TestMethod]
        public void Resize_InitPython_ShouldReturnResizedImage()
        {
            const int width = 100;
            const int height = 100;
            const string url = "https://www.podcastinit.com/wp-content/cache/podlove/7a/cf0997aacd0c64b37245739c9c9085/podcast-__init__python_original.png";
            const string podcastTitle = "Podcast.__init__(\'Python\')";

            var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle }).Result;

            Assert.AreEqual("podcastinitpython.png", resizedImage.ImageName);
        }

        [TestMethod]
        public void Resize_BrentOzarUnlimited_ShouldReturnNullAsSmallerThanFiveHundredPixels()
        {
            const int width = 122;
            const int height = 122;
            const string url = "https://www.brentozar.com/wp-content/plugins/powerpress/rss_default.jpg";
            const string podcastTitle = "Brent Ozar Unlimited®";

            var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle }).Result;

            Assert.AreEqual("brent_ozar_unlimited.jpeg", resizedImage.ImageName);
        }
    }
}
