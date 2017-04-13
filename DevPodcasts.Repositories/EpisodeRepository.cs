using DevPodcasts.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DevPodcasts.Dtos;

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

        public void AddRange(IEnumerable<EpisodeDto> dtos)
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

                _context.SaveChanges();
            }
        }
    }
}
