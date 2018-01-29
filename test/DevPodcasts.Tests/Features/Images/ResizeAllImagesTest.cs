using DevPodcasts.DataLayer.Models;
using DevPodcasts.Web.Features.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DevPodcasts.Tests.Features.Images
{
    [TestClass]
    public class ResizeAllImagesTest : TestBase
    {
        // We don't need to run this test continuously
        [Ignore] 
        [TestMethod]
        public void ResizeAllPodcastImages()
        {
            var context = new ApplicationDbContext();
            const string containerName = "images-test";
            const int width = 500;
            const int height = 500;

            var podcasts = context.Podcasts
                .Where(podcast => podcast.IsApproved == true && podcast.ResizedImageUrl == null && podcast.ImageUrl != null && !podcast.Title.Contains("svg"))
                .ToList();

            foreach (var podcast in podcasts)
            {
                var url = podcast.ImageUrl;

                var resizedImage = Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcast.Title }).Result;

                if (resizedImage == null)
                    continue;

                var resizedImageUrl = Mediator.Send(new UploadToBlob.Command { Bytes = resizedImage.ImageBytes, BlobReference = resizedImage.ImageName, ContainerName = containerName, ContentType = resizedImage.ContentType }).Result;

                podcast.ResizedImageUrl = resizedImageUrl;
            }

            context.SaveChanges();
        }
    }
}
