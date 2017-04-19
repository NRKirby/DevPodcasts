using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Episode;
using System;
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
            return new EpisodeDetailViewModel
            {
                Id = episode.Id,
                Title = episode.Title,
                Summary = StripHtml(episode.Summary),
                AudioUrl = episode.AudioUrl,
                EpisodeUrl = episode.EpisodeUrl,
                DatePublished = episode.DatePublished?.ToShortDateString()
            };
        }

        public static string StripHtml(string input)
        {
            var replace = Regex.Replace(input, "<.*?>", string.Empty);
            replace = Regex.Replace(replace, "&nbsp;", " ");
            return replace;
        }
    }
}
