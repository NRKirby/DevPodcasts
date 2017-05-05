using DevPodcasts.ViewModels.Episode;

namespace DevPodcasts.ServiceLayer.Episode
{
    public interface IEpisodeService
    {
        EpisodeDetailViewModel GetEpisodeDetail(int id);

        bool EpisodeExists(int episodeId);
    }
}
