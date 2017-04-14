using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class EpisodeUpdater
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly EpisodeRepository _episodeRepository;
        private readonly RssService _parser;

        public EpisodeUpdater()
        {
            _podcastRepository = new PodcastRepository();
            _episodeRepository = new EpisodeRepository();
            _parser = new RssService();
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
                var newEpisodes = _parser.GetNewEpisodes(podcast);
                await _episodeRepository.AddRange(newEpisodes);
            }
        }
    }
}
