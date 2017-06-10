using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.Logging;
using DevPodcasts.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public class EpisodeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public EpisodeRepository(ILogger logger)
        {
            _context = new ApplicationDbContext();
            _logger = logger;
        }

        public IEnumerable<EpisodeDto> GetAllEpisodes(int podcastId)
        {
            return _context.Episodes
                .Where(i => i.PodcastId == podcastId)
                .OrderByDescending(i => i.DatePublished)
                .Select(i => new EpisodeDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    DatePublished = i.DatePublished
                });
        }

        public EpisodeDto GetEpisode(int episodeId)
        {
            var episode = _context.Episodes.Single(i => i.Id == episodeId);
            return new EpisodeDto
            {
                Id = episode.Id,
                Title = episode.Title,
                Summary = episode.Summary,
                AudioUrl = episode.AudioUrl,
                EpisodeUrl = episode.EpisodeUrl,
                DatePublished = episode.DatePublished
            };
        }

        public DateTime? GetMostRecentEpisodeDate(int podcastId)
        {
            return _context.Episodes.Where(i => i.PodcastId == podcastId).Select(i => i.DatePublished).Max();
        }

        public bool EpisodeExists(int episodeId)
        {
            return _context.Episodes.Any(i => i.Id == episodeId);
        }

        public int EpisodeCount()
        {
            return _context.Episodes.Count();
        }

        public async Task<int> AddRange(IEnumerable<EpisodeDto> dtos)
        {
            int addedCount = 0;
            foreach (var dto in dtos)
            {
                var podcast = _context.Podcasts.Single(i => i.Id == dto.PodcastId);
                
                var episode = new Episode
                {
                    EpisodeId = dto.EpisodeId,
                    Title = dto.Title,
                    Summary = dto.Summary,
                    AudioUrl = dto.AudioUrl,
                    EpisodeUrl = dto.EpisodeUrl,
                    DatePublished = dto.DatePublished,
                    DateCreated = DateTime.Now
                };

                var episodeExistsForPodcast = _context.Podcasts
                    .Single(i => i.Id == dto.PodcastId)
                    .Episodes
                    .Any(i => i.Title == episode.Title);

                if (!episodeExistsForPodcast)
                {
                    podcast.Episodes.Add(episode);
                    _logger.Info($"{podcast.Title} - {episode.Title} added");
                    addedCount++;
                }
            }

            await _context.SaveChangesAsync();
            return addedCount;
        }

        public int AddRangeSync(IEnumerable<EpisodeDto> dtos)
        {
            int addedCount = 0;
            foreach (var dto in dtos)
            {
                var podcast = _context.Podcasts.Single(i => i.Id == dto.PodcastId);

                var episode = new Episode
                {
                    EpisodeId = dto.EpisodeId,
                    Title = dto.Title,
                    Summary = dto.Summary,
                    AudioUrl = dto.AudioUrl,
                    EpisodeUrl = dto.EpisodeUrl,
                    DatePublished = dto.DatePublished,
                    DateCreated = DateTime.Now
                };

                var episodeExistsForPodcast = _context.Podcasts
                    .Single(i => i.Id == dto.PodcastId)
                    .Episodes
                    .Any(i => i.Title == episode.Title);

                if (!episodeExistsForPodcast)
                {
                    podcast.Episodes.Add(episode);
                    _logger.Info($"{podcast.Title} - {episode.Title} added");
                    addedCount++;
                }
            }

            _context.SaveChanges();
            return addedCount;
        }

        public IEnumerable<RecentEpisode> GetMostRecentEpisodes(int numberOfEpisodes)
        {
            var recentEpisodes = _context
                .Episodes
                .OrderByDescending(i => i.Id)
                .Take(numberOfEpisodes)
                .ToList();

            var recentEpisodesList = new List<RecentEpisode>();

            foreach (var episode in recentEpisodes)
            {
                recentEpisodesList.Add(new RecentEpisode
                {
                    Id = episode.Id,
                    Title = episode.Title,
                    PodcastTitle = episode.Podcast.Title
                });
            }

            return recentEpisodesList;
        }
    }
}
