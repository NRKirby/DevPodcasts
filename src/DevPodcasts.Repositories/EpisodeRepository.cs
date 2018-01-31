using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.Logging;
using DevPodcasts.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;

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

        public DateTime? GetMostRecentEpisodeDate(int podcastId)
        {
            return _context
                .Episodes
                .AsNoTracking()
                .Where(i => i.PodcastId == podcastId)
                .Select(i => i.DatePublished).Max();
        }

        public bool EpisodeExists(int episodeId)
        {
            return _context.Episodes
                .AsNoTracking()
                .Any(i => i.Id == episodeId);
        }

        public int EpisodeCount()
        {
            return _context
                .Episodes
                .AsNoTracking()
                .Count();
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
            IEnumerable<RecentEpisode> episodes;
            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                episodes = db.Query<RecentEpisode>(GetQuery());
            };
            return episodes;
        }

        private static string GetQuery()
        {
            return @"SELECT TOP 10 Episodes.Id as Id, Episodes.Title as Title, Podcasts.Title AS PodcastTitle 
                     FROM Episodes
                     INNER JOIN Podcasts
                     ON Episodes.PodcastId = Podcasts.Id
                     ORDER BY DatePublished DESC";
        }
    }

}
