using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public class PodcastRepository : IPodcastRepository
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

        public bool PodcastExists(string rssFeedUrl)
        {
            return _context.Podcasts.Any(i => i.FeedUrl == rssFeedUrl);
        }

        public IEnumerable<PodcastViewModel> GetUnapprovedPodcasts()
        {
            return _context.Podcasts
                .Where(i => i.IsApproved == null)
                .OrderBy(i => i.DateCreated)
                .Select(i => new PodcastViewModel
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                Title = i.Title,
                SiteUrl = i.SiteUrl,
                DateAdded = i.DateCreated,
            });
        }

        public async Task Reject(int podcastId)
        {
            var podcast = _context.Podcasts.Single(i => i.Id == podcastId);
            podcast.IsApproved = false;
            await _context.SaveChangesAsync();
        }

        public PodcastDto GetPodcast(int podcastId)
        {
            var obj = _context.Podcasts
                .Where(i => i.Id == podcastId)
                .Select(i => new {i.Id, i.FeedUrl, i.Title, i.SiteUrl})
                .Single();

            return new PodcastDto
            {
                Id = obj.Id,
                FeedUrl = obj.FeedUrl,
                Title = obj.Title,
                SiteUrl = obj.SiteUrl
            };
        }

        public void AddEpisodesToPodcast(PodcastDto dto)
        {
            var podcast = _context.Podcasts.Single(i => i.Id == dto.Id);

            foreach (var episodeDto in dto.Episodes)
            {
                var episode = new Episode
                {
                    EpisodeId = episodeDto.EpisodeId,
                    Title = episodeDto.Title,
                    Summary = episodeDto.Summary,
                    AudioUrl = episodeDto.AudioUrl,
                    EpisodeUrl = episodeDto.EpisodeUrl,
                    DatePublished = episodeDto.DatePublished,
                    DateCreated = episodeDto.DateCreated
                };
                podcast.Episodes.Add(episode);
            }
            podcast.IsApproved = true;
            podcast.DateApproved = DateTime.Now;
            _context.SaveChanges();
        }

        public IEnumerable<PodcastDto> GetDistinctPodcasts()
        {
            return _context.Podcasts
                .Distinct()
                .Select(i => new PodcastDto
                {
                    Id = i.Id,
                    FeedUrl = i.FeedUrl
                });
        }
    }
}
