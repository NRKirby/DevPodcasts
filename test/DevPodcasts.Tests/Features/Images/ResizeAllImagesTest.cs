using DevPodcasts.DataLayer.Models;
using DevPodcasts.Web.Features.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DevPodcasts.Tests.Features.Images
{
    [TestClass]
    public class ResizeAllImagesTest : TestBase
    {
        [TestMethod]
        public void ResizeAllPodcastImages()
        {
            var context = new ApplicationDbContext();
            const string containerName = "podcast-images";
            const int width = 500;
            const int height = 500;

            var podcasts = context.Podcasts
                .Where(podcast => podcast.IsApproved == true 
                    && podcast.ResizedImageUrl == null 
                    && podcast.ImageUrl != null 
                    && !podcast.Title.Contains("svg"))
                .ToList();

            var resizeHandler = new Resize.CommandHandler();
            var uploadHandler = new UploadToBlob.CommandHandler();

            foreach (var podcast in podcasts)
            {
                var url = podcast.ImageUrl;

                var resizedImage = resizeHandler.Handle(new Resize.Command {
                    Width = width,
                    Height = height,
                    ImageUrl = url,
                    PodcastTitle = podcast.Title });

                if (resizedImage == null)
                    continue;

                var resizedImageUrl = uploadHandler.Handle(new UploadToBlob.Command {
                    Bytes = resizedImage.ImageBytes,
                    BlobReference = resizedImage.ImageName,
                    ContainerName = containerName,
                    ContentType = resizedImage.ContentType });

                podcast.ResizedImageUrl = resizedImageUrl;
            }

            context.SaveChanges();
        }
    }
}
