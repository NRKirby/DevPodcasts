using DevPodcasts.DataLayer.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

namespace ImageResizing
{
    class Program
    {
        static void Main(string[] args)
        {
            var imageUrls = GetImageUrls();
            var images = ImagesGreaterThanFromUrls(imageUrls, 500);
        }

        private static IEnumerable<string> GetImageUrls()
        {
            var context = new ApplicationDbContext();
            var imageUrls = context.Podcasts.Where(p => p.ResizedImageUrl == null && p.ImageUrl != null).Select(p => p.ImageUrl).ToList();

            return imageUrls;
        }

        public static IEnumerable<Image> ImagesGreaterThanFromUrls(IEnumerable<string> urls, int width)
        {
            Bitmap bitmap;
            Stream stream;
            var images = new List<Image>();
            var client = new WebClient();

            foreach (var url in urls)
            {
                try
                {
                    stream = client.OpenRead(url);
                    bitmap = new Bitmap(stream);
                    if (bitmap.Width > width)
                    {
                        images.Add(bitmap);
                    }
                }
                catch
                {

                }
            }
            client.Dispose();
            return images;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }


}
