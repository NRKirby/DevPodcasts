using DevPodcasts.Web.Features.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DevPodcasts.Tests.Features.Images
{
    [TestClass]
    public class UploadToBlobTests : TestBase
    {
        [TestMethod]
        public void UploadToBlob_AgileInThreeMinutes()
        {
            const int width = 500;
            const int height = 500;
            const string url = "https://agilein3minut.es/images/ai3m.png";
            const string containerName = "images-test";
            const string podcastTitle = "Agile in 3 Minutes";

            var resizedImage =  Mediator.Send(new Resize.Command { Width = width, Height = height, ImageUrl = url, PodcastTitle = podcastTitle}).Result;

            var result = Mediator.Send(new UploadToBlob.Command { Bytes = resizedImage.ImageBytes, BlobReference = resizedImage.ImageName, ContainerName = containerName, ContentType = resizedImage.ContentType  }).Result;

            Assert.AreEqual($"https://devpodcasts.blob.core.windows.net/{containerName}/agile_in_3_minutes.png", result);
        }
    }
}
