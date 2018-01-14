using DevPodcasts.Repositories;

namespace DevPodcasts.ServiceLayer.Episode
{
    public class EpisodeService
    {
        private readonly EpisodeRepository _episodeRepository;

        public EpisodeService(EpisodeRepository episodeRepository)
        {
            _episodeRepository = episodeRepository;
        }

        public bool EpisodeExists(int episodeId)
        {
            return _episodeRepository.EpisodeExists(episodeId);
        }
    }
}
