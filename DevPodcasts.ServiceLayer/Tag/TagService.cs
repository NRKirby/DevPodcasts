using System.Collections.Generic;
using System.Linq;
using DevPodcasts.DataLayer.Models;

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
                    Slug = i.Slug
                });
        }
    }

    public class TagItemViewModel
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
