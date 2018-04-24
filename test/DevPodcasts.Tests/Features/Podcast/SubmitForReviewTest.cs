using DevPodcasts.DataLayer.Models;
using DevPodcasts.Logging;
using DevPodcasts.Models;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ServiceLayer.RSS;
using DevPodcasts.Web.Features.Podcast;
using FakeItEasy;
using System.Linq;
using Xunit;

namespace DevPodcasts.Tests.Features.Podcast
{
    public class SubmitForReviewTest : TestBase
    {
        [Fact]
        public async void InvalidUrl_ShouldReturnInvalidUrlResult()
        {
            var command = new SubmitForReview.Command { RssFeedUrl = "https://" };

            var handler = new SubmitForReview.CommandHandler(A.Fake<ApplicationDbContext>(), A.Fake<RssParser>(), A.Fake<PodcastEmailService>(), A.Fake<ILogger>());
            var result = await handler.Handle(command);

            Assert.Equal(SuccessResult.InvalidUrl, result.SuccessResult);
        }

        [Fact]
        public async void AllThingsGit_ShouldStorePodcastInDatabaseForReview()
        {
            var command = new SubmitForReview.Command { RssFeedUrl = "https://www.allthingsgit.com/rss.xml" };

            var handler = new SubmitForReview.CommandHandler(Context, A.Fake<RssParser>(), A.Fake<PodcastEmailService>(), A.Fake<ILogger>());
            var result = await handler.Handle(command);

            var podcast = Context.Podcasts.SingleOrDefault(p => p.Title == "All Things Git");
            Assert.Equal(SuccessResult.Success, result.SuccessResult);
            Assert.NotNull(podcast);
            Assert.Equal("All Things Git", podcast.Title);
            Assert.Null(podcast.IsApproved);
        }

        [Fact]
        public async void AllThingsGit_ShouldReturnAlreadyInDatabaseResult()
        {
            var podcast = new DataLayer.Models.Podcast
            {
                Title = "All Things Git",
                FeedUrl = "https://www.allthingsgit.com/rss.xml",
                IsApproved = true
            };
            Context.Podcasts.Add(podcast);
            await Context.SaveChangesAsync();

            var command = new SubmitForReview.Command { RssFeedUrl = "https://www.allthingsgit.com/rss.xml" };

            var handler = new SubmitForReview.CommandHandler(Context, A.Fake<RssParser>(), A.Fake<PodcastEmailService>(), A.Fake<ILogger>());
            var result = await handler.Handle(command);

            Assert.Equal(SuccessResult.AlreadyExists, result.SuccessResult);
        }
    }
}
