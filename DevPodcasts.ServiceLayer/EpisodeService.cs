﻿using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Episode;

namespace DevPodcasts.ServiceLayer
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IEpisodeRepository _episodeRepository;

        public EpisodeService(IEpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public EpisodeDetailViewModel GetEpisodeDetail(int id)
        {
            var episode = _episodeRepository.GetEpisode(id);
            return new EpisodeDetailViewModel
            {
                Title = episode.Title,
                Summary = episode.Summary,
                AudioUrl = episode.AudioUrl,
                EpisodeUrl = episode.EpisodeUrl,
                DatePublished = episode.DatePublished?.ToShortDateString()
            };

        }
    }
}