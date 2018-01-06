using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Episode;
using System;

namespace DevPodcasts.ServiceLayer.Episode
{
    public class EpisodeService
    {
        private readonly EpisodeRepository _episodeRepository;

        public EpisodeService(EpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public EpisodeDetailViewModel GetEpisodeDetail(int id)
        {
            var episode = _episodeRepository.GetEpisode(id);
            var viewModel = new EpisodeDetailViewModel
            {
                Id = episode.Id,
                PodcastId = episode.PodcastId,
                Title = episode.Title,
                AudioUrl = episode.AudioUrl,
                EpisodeUrl = episode.EpisodeUrl,
                PodcastTitle = episode.PodcastTitle,
                Summary = episode.Summary
            };

            if (episode.DatePublished != null)
            {
                viewModel.DatePublished = (DateTime)episode.DatePublished;
            }

            return viewModel;
        }

        public bool EpisodeExists(int episodeId)
        {
            return _episodeRepository.EpisodeExists(episodeId);
        }
    }
}
