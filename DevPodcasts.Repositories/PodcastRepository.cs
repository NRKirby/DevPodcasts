using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using System;
using System.Collections.Generic;
using DevPodcasts.ViewModels.Admin;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public class PodcastRepository
    {
        private readonly ApplicationDbContext _context;

        public PodcastRepository()
        {
            _context = new ApplicationDbContext();
        }

        public async Task Add(PodcastDto dto)
        {
            var podcast = new Podcast
            {
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                FeedUrl = dto.FeedUrl,
                SiteUrl = dto.SiteUrl,
                DateCreated = DateTime.Now
            };
            _context.Podcasts.Add(podcast);
            await _context.SaveChangesAsync();
        }

        public bool PodcastExists(string title)
        {
            return _context.Podcasts.Any(i => i.Title == title);
        }

        public IEnumerable<PodcastViewModel> GetUnapprovedPodcasts()
        {
            return _context.Podcasts
                .Where(i => i.IsApproved == null)
                .OrderByDescending(i => i.DateCreated)
                .Select(i => new PodcastViewModel
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Title = i.Title,
                SiteUrl = i.SiteUrl,
                DateAdded = i.DateCreated,
                ApprovalState = ApprovalState.Unapproved
            });
        }
    }
}
