using DevPodcasts.DataLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DevPodcasts.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesRepository()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.OrderBy(i => i.Name);
        }
    }
}
