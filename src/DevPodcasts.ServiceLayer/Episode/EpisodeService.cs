using System;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Episode;
using System.Text.RegularExpressions;

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
            var summary = episode.Summary;
            var viewModel = new EpisodeDetailViewModel
            {
                Id = episode.Id,
                PodcastId = episode.PodcastId,
                Title = episode.Title,
                AudioUrl = episode.AudioUrl,
                EpisodeUrl = episode.EpisodeUrl,
                PodcastTitle = episode.PodcastTitle
            };

            if (episode.DatePublished != null)
            {
                viewModel.DatePublished = (DateTime)episode.DatePublished;
            }

            if (summary != null)
            {
                viewModel.Summary = StripHtml(summary);
            }

            return viewModel;
        }

        public bool EpisodeExists(int episodeId)
        {
            return _episodeRepository.EpisodeExists(episodeId);
        }

        public static string StripHtml(string input)
        {
            var replace = Regex.Replace(input, "<.*?>", string.Empty);
            replace = Regex.Replace(replace, "&nbsp;", " ");
            return replace;
        }
    }
}
