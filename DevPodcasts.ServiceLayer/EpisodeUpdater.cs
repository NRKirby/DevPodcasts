using DevPodcasts.Repositories;

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

        public void Update()
        {
            var podcasts = _podcastRepository.GetDistinctPodcasts();

            foreach (var podcast in podcasts)
            {
                var newEpisodes = _parser.GetNewEpisodes(podcast);
                _episodeRepository.AddRange(newEpisodes);
            }
        }
    }
}
