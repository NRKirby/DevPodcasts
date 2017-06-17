﻿using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.ViewModels.Admin;
using System;
using DevPodcasts.ViewModels.Home;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DevPodcasts.ViewModels.Podcast;

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

        public IEnumerable<FeaturedPodcast> GetFeaturedPodcasts(int numberOfPodcasts)
        {
            return _context
                .Podcasts
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .OrderBy(i => Guid.NewGuid())
                .Take(numberOfPodcasts)
                .Select(i => new FeaturedPodcast
                {
                    PodcastId = i.Id,
                    Title = i.Title,
                    Description = i.Description
                });
        }

        public bool PodcastExists(string rssFeedUrl)
        {
            return _context
                .Podcasts
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .Any(i => i.FeedUrl == rssFeedUrl);
        }

        public int GetTotalPodcasts()
        {
            return _context
                .Podcasts
                .AsNoTracking()
                .Count(i => i.IsApproved == true);
        }

        public IEnumerable<PodcastViewModel> GetUnapprovedPodcasts()
        {
            return _context
                .Podcasts
                .AsNoTracking()
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
            var podcast = _context
                .Podcasts
                .Where(i => i.Id == podcastId)
                .Include(i => i.Tags)
                .Single();

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
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .Any(i => i.Id == podcastId);
        }

        public IEnumerable<PodcastSearchResultViewModel> Search(string query)
        {
            IOrderedQueryable<Podcast> podcastQuery;
            if (query == null) // return all approved podcasts
            {
                podcastQuery = _context
                    .Podcasts
                    .AsNoTracking()
                    .Where(i => i.IsApproved == true)
                    .OrderBy(i => i.Title);
            }
            else
            {
                podcastQuery = _context
                    .Podcasts
                    .AsNoTracking()
                    .Where(i => i.IsApproved == true && i.Title.Contains(query) || 
                                i.IsApproved == true && i.Description.Contains(query))
                    .OrderBy(i => i.Title);
            }

            return podcastQuery
                .Select(i => new PodcastSearchResultViewModel
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
            return _context
                .Podcasts
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .Select(i => new PodcastDto
            {
                Id = i.Id,
                Title = i.Title
            })
            .OrderBy(i => i.Title);
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
            var podcast = _context
                .Podcasts
                .Where(i => i.Id == dto.Id)
                .Include(i => i.Tags)
                .FirstOrDefault();

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

                var episodeAlreadyExists = _context.Episodes.Any(i => i.Title == episode.Title);
                if (!episodeAlreadyExists)
                {
                    podcast.Episodes.Add(episode);
                }
                else
                {
                    Console.Write("Episode already exists");
                }
            }
            podcast.IsApproved = true;
            podcast.DateApproved = DateTime.Now;
            _context.SaveChanges();
        }

        public IEnumerable<PodcastDto> GetDistinctPodcasts()
        {
            return _context
                .Podcasts
                .AsNoTracking()
                .Where(i => i.IsApproved == true)
                .Distinct()
                .Select(i => new PodcastDto
                {
                    Id = i.Id,
                    FeedUrl = i.FeedUrl,
                    Title = i.Title
                });
        }

        public async Task DeletePodcast(int podcastId)
        {
            var podcastToDelete = _context
                .Podcasts
                .AsNoTracking()
                .SingleOrDefault(p => p.Id == podcastId);

            // delete in disconnected state 
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Entry(podcastToDelete).State = EntityState.Deleted;

                await ctx.SaveChangesAsync();
            }
        }
    }
}
