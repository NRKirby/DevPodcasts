using DevPodcasts.DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DevPodcasts.Repositories
{
    public class TagsRepository : ITagsRepository
    {
        private readonly ApplicationDbContext _context;

        public TagsRepository()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<Tag> GetAll()
        {
            return _context.Tags.OrderBy(i => i.Name);
        }
    }
}
