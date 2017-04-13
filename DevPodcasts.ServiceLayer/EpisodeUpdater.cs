using DevPodcasts.Repositories;

namespace DevPodcasts.ServiceLayer
{
    public class EpisodeUpdater
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly EpisodeRepository _episodeRepository;

        public EpisodeUpdater()
        {
            _podcastRepository = new PodcastRepository();
            _episodeRepository = new EpisodeRepository();
        }

        public void Update()
        {
            var podcasts = _podcastRepository.GetDistinctPodcasts();

            foreach (var p in podcasts)
            {
                var mostRecentDate = _episodeRepository.GetMostRecentDate(p.Id);
            }
        }
    }
}
