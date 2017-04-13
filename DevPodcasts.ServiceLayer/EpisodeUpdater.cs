using DevPodcasts.Repositories;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class EpisodeUpdater
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly EpisodeRepository _episodeRepository;
        private readonly RssParser _parser;

        public EpisodeUpdater()
        {
            _podcastRepository = new PodcastRepository();
            _episodeRepository = new EpisodeRepository();
            _parser = new RssParser();
        }

        public async Task Update()
        {
            var podcasts = _podcastRepository.GetDistinctPodcasts();

            foreach (var podcast in podcasts)
            {
                var newEpisodes = _parser.GetNewEpisodes(podcast);
                await _episodeRepository.AddRange(newEpisodes);
            }
        }
    }
}
