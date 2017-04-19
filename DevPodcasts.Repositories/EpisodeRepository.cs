using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public class EpisodeRepository : IEpisodeRepository
    {
        private readonly ApplicationDbContext _context;

        public EpisodeRepository()
        {
            _context = new ApplicationDbContext();
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

        public async Task AddRange(IEnumerable<EpisodeDto> dtos)
        {
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

                podcast.Episodes.Add(episode);
            }

            await _context.SaveChangesAsync();
        }
    }
}
