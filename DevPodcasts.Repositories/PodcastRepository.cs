using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        public IEnumerable<PodcastDto> GetFeaturedPodcasts(int numberOfPodcasts)
        {
            return _context
                .Podcasts
                .Where(i => i.IsApproved == true)
                .OrderBy(i => Guid.NewGuid())
                .Take(numberOfPodcasts)
                .Select(i => new PodcastDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description
                });
        }

        public bool PodcastExists(string rssFeedUrl)
        {
            return _context
                .Podcasts
                .Where(i => i.IsApproved == true)
                .Any(i => i.FeedUrl == rssFeedUrl);
        }

        public int GetTotalPodcasts()
        {
            return _context
                .Podcasts
                .Count(i => i.IsApproved == true);
        }

        public IEnumerable<PodcastViewModel> GetUnapprovedPodcasts()
        {
            return _context
                .Podcasts
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

        public async Task SaveCategories(int podcastId, IEnumerable<int> categoryIds)
        {
            var podcast = _context.Podcasts.Include("Tags").Single(i => i.Id == podcastId);
            foreach (var categoryId in categoryIds)
            {
                var category = _context.Tags.Find(categoryId);
                podcast.Tags.Add(category);
            }
            await _context.SaveChangesAsync();
        }

        public bool PodcastExists(int podcastId)
        {
            return _context
                .Podcasts
                .Where(i => i.IsApproved == true)
                .Any(i => i.Id == podcastId);
        }

        public IEnumerable<PodcastSearchResultDto> Search(string query)
        {
            return _context
                .Podcasts
                .Where(i => i.IsApproved == true && i.Title.Contains(query) || i.Description.Contains(query))
                .OrderBy(i => i.Title)
                .Select(i => new PodcastSearchResultDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    NumberOfEpisodes = i.Episodes.Count,
                    Description = i.Description,
                    ImageUrl = i.ImageUrl
                });
        }

        public IEnumerable<PodcastDto> GetAllPodcasts()
        {
            return _context.Podcasts.Select(i => new PodcastDto
            {
                Id = i.Id,
                Title = i.Title
            }).OrderBy(i => i.Title);
        }

        public PodcastDto GetPodcastForEdit(int podcastId)
        {
            return _context
                .Podcasts
                .Where(i => i.Id == podcastId)
                .Select(i => new PodcastDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    ImageUrl = i.ImageUrl,
                    FeedUrl = i.FeedUrl,
                    SiteUrl = i.SiteUrl,
                    Tags = i.Tags.Select(t => new TagDto
                    {
                        Id = t.TagId,
                        Name = t.Name  
                    })
                })
                .Single();
        }

        public async Task UpdatePodcast(PodcastDto dto)
        {
            var podcast = _context.Podcasts.Include("Tags").FirstOrDefault(i => i.Id == dto.Id);

            if (podcast != null)
            {
                podcast.Title = dto.Title;
                podcast.Description = dto.Description;
                podcast.ImageUrl = dto.ImageUrl;
                podcast.FeedUrl = dto.FeedUrl;
                podcast.SiteUrl = dto.SiteUrl;

                var selectedTagIds = dto
                    .Tags
                    .Select(i => i.Id);
                await _context.Entry(podcast).Collection(p => p.Tags).LoadAsync();

                var newTags = _context
                    .Tags
                    .Where(t => selectedTagIds.Contains(t.TagId))
                    .ToList();
                podcast.Tags = newTags;
            }

            await _context.SaveChangesAsync();
        }

        public PodcastDto GetPodcast(int podcastId)
        {
            var podcast = _context
                .Podcasts
                .Single(i => i.Id == podcastId);

            return new PodcastDto
            {
                Id = podcast.Id,
                FeedUrl = podcast.FeedUrl,
                Title = podcast.Title,
                SiteUrl = podcast.SiteUrl,
                Description = podcast.Description,
                ImageUrl = podcast.ImageUrl
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
            return _context
                .Podcasts
                .Where(i => i.IsApproved == true)
                .Distinct()
                .Select(i => new PodcastDto
                {
                    Id = i.Id,
                    FeedUrl = i.FeedUrl,
                    Title = i.Title
                });
        }
    }
}
