using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Episode;
using System.Text.RegularExpressions;

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
            var summary = episode.Summary;
            var viewModel = new EpisodeDetailViewModel
            {
                Id = episode.Id,
                Title = episode.Title,
                AudioUrl = episode.AudioUrl,
                EpisodeUrl = episode.EpisodeUrl,
                DatePublished = episode.DatePublished?.ToShortDateString()
            };
            if (summary != null)
                viewModel.Summary = StripHtml(summary);

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
