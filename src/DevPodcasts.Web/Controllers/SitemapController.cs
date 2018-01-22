using DevPodcasts.Web.Features.Sitemap;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class SitemapController : Controller
    {
        public ActionResult Index()
        {
            var sitemapItems = new List<SitemapItem>
            {
                new SitemapItem("https://devpodcasts.net/home/index", changeFrequency: SitemapChangeFrequency.Always, priority: 1.0),
                new SitemapItem("https://devpodcasts.net/home/about", lastModified: DateTime.Now),
            };

            return new SitemapResult(sitemapItems);
        }
    }
}