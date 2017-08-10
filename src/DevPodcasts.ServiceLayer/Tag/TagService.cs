using DevPodcasts.DataLayer.Models;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Linq;
using DevPodcasts.ViewModels.Tags;

namespace DevPodcasts.ServiceLayer.Tag
{
    public class TagService
    {
        private readonly ApplicationDbContext _context;

        public TagService()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<TagItemViewModel> GetTags()
        {
            return _context
                .Tags
                .OrderBy(i => i.Name)
                .Select(i => new TagItemViewModel
                {
                    Name = i.Name,
                    Slug = i.Slug,
                    NumberOfPodcasts = i.Podcasts.Count
                });
        }

        public TagResultViewModel GetTaggedPodcasts(string tagSlug)
        {
            var tagName = _context
                .Tags
                .Where(t => t.Slug == tagSlug)
                .Select(i => i.Name)
                .FirstOrDefault();
            
            var podcasts = _context
                .Podcasts
                .Where(p => p.Tags.Any(t => t.Slug == tagSlug))
                .OrderBy(p => p.Title)
                .Select(i => new PodcastSearchResultViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    ImageUrl = i.ImageUrl,
                    Description = i.Description,
                    NumberOfEpisodes = i.Episodes.Count
                });

            return new TagResultViewModel
            {
                TagName = tagName,
                SearchResults = podcasts
            };
        }
    }

    public class TagItemViewModel
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public int NumberOfPodcasts { get; set; }
    }
}
