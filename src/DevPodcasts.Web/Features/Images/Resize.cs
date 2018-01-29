using System;
using ImageProcessor;
using MediatR;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DevPodcasts.Web.Features.Images
{
    public class Resize
    {
        public class Command : IRequest<ResizedImage>
        {
            public string ImageUrl;
            public string PodcastTitle { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, ResizedImage>
        {
            public ResizedImage Handle(Command message)
            {
                if (message.ImageUrl.Contains(".svg"))
                    return null;

                var webClient = new WebClient();
                byte[] imageBytes;
                try
                {
                    imageBytes = webClient.DownloadData(message.ImageUrl);
                }
                catch
                {
                    return null;
                }

                var resizedImage = new ResizedImage();

                using (var inStream = new MemoryStream(imageBytes))
                {
                    using (var outStream = new MemoryStream())
                    {
                        using (var imageFactory = new ImageFactory())
                        {
                            imageFactory.Load(inStream);
                            var image = imageFactory.Image;

                            // only resize images which are larger than resize size
                            var max = Math.Max(image.Width, image.Height);
                            if (max < message.Width)
                            {
                                return null;
                            }

                            imageFactory
                                .Resize(new System.Drawing.Size(message.Width, message.Height))
                                .Save(outStream);
                        }

                        resizedImage.ImageBytes = GetBytesFromStream(outStream);
                        resizedImage.ContentType = GetContentType(message.ImageUrl);
                        resizedImage.ImageName = GetImageNameAndExtension(message.PodcastTitle, resizedImage.ContentType);
                    }
                }

                return resizedImage;
            }

            private static string GetImageNameAndExtension(string podcastTitle, string contentType)
            {
                var imageExtension = contentType == "image/jpeg" ? ".jpeg" : ".png";
                var imageName = Regex.Replace(podcastTitle, "[.|%|'|(|)|_|®|@]", "").Replace(" ","_").ToLower();

                return imageName + imageExtension;
            }

            private static string GetContentType(string imageUrl)
            {
                string contentType = null;
                if (imageUrl.Contains("jpeg") || imageUrl.Contains("jpg"))
                {
                    contentType = "image/jpeg";
                }
                else if (imageUrl.Contains("png"))
                {
                    contentType = "image/png";
                }

                return contentType;
            }

            public static byte[] GetBytesFromStream(Stream input)
            {
                using (var ms = new MemoryStream())
                {
                    input.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public class ResizedImage
        {
            public byte[] ImageBytes { get; set; }
            public string ContentType { get; set; }
            public string ImageName { get; set; }
        }
    }
}