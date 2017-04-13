using DevPodcasts.DataLayer.Models;
using System;
using System.Linq;

namespace DevPodcasts.Repositories
{
    public class EpisodeRepository
    {
        private readonly ApplicationDbContext _context;

        public EpisodeRepository()
        {
            _context = new ApplicationDbContext();
        }

        public DateTime? GetMostRecentDate(int podcastId)
        {
            return _context.Episodes.Where(i => i.PodcastId == podcastId).Select(i => i.DatePublished).Max();
        }
    }
}
