using DevPodcasts.DataLayer.Models;
using System.Collections.Generic;

namespace DevPodcasts.Repositories
{
    public interface ICategoriesRepository
    {
        IEnumerable<Tag> GetAll();
    }
}
