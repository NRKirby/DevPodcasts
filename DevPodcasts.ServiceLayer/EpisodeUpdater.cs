using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class EpisodeUpdater
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IRssService _rssService;

        public EpisodeUpdater(IPodcastRepository podcastRepository,
            IEpisodeRepository episodeRepository,
            IRssService rssService)
        {
            _podcastRepository = podcastRepository;
            _episodeRepository = episodeRepository;
            _rssService = rssService;
        }

        public void Update()
        {
            Task.Run(async () =>
            {
                var podcasts = _podcastRepository.GetDistinctPodcasts();
                await UpdatePodcasts(podcasts);
            });
        }

        private async Task UpdatePodcasts(IEnumerable<PodcastDto> podcasts)
        {
            foreach (var podcast in podcasts)
            {
                var newEpisodes = _rssService.GetNewEpisodes(podcast);
                await _episodeRepository.AddRange(newEpisodes);
            }
        }
    }
}
