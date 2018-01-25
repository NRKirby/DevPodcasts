using DevPodcasts.DataLayer.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Sitemap
{
    public class GenerateSitemapItems
    {
        public class Query : IRequest<IEnumerable<SitemapItem>>
        {

        }

        public class QueryHandler : IAsyncRequestHandler<Query, IEnumerable<SitemapItem>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<SitemapItem>> Handle(Query message)
            {
                var sitemapItems = new List<SitemapItem>
                {
                    new SitemapItem("https://devpodcasts.net"),
                    new SitemapItem("https://devpodcasts.net/home/login", priority: 0.4, changeFrequency: SitemapChangeFrequency.Never),
                    new SitemapItem("https://devpodcasts.net/home/register", priority: 0.4, changeFrequency: SitemapChangeFrequency.Never),
                    new SitemapItem("https://devpodcasts.net/tags"),
                    new SitemapItem("https://devpodcasts.net/search"),
                    new SitemapItem("https://devpodcasts.net/podcasts", priority: 0.9)
                };

                var episodeIds = _context.Episodes.OrderByDescending(episode => episode.Id).Select(episode => episode.Id).ToList();

                foreach (var id in episodeIds)
                {
                    sitemapItems.Add(new SitemapItem($"https://devpodcasts.net/episode/detail/{id}", priority: 1, changeFrequency: SitemapChangeFrequency.Never));
                }

                var podcastIds = _context.Podcasts.Where(podcast => podcast.IsApproved == true).Select(podcast => podcast.Id).ToList();

                foreach (var id in podcastIds)
                {
                    sitemapItems.Add(new SitemapItem($"https://devpodcasts.net/podcasts/detail/{id}", priority: 0.9));
                }

                var tags = _context.Tags.Select(tag => new { tag.TagId, tag.Slug }).ToList();

                foreach (var id in tags.OrderBy(tag => tag.Slug).Select(tag => tag.TagId))
                {
                    var slug = tags.Single(tag => tag.TagId == id).Slug;
                    sitemapItems.Add(new SitemapItem($"https://devpodcasts.net/podcasts/tagged/{slug}"));
                }

                return sitemapItems;
            }
        }


    }
}