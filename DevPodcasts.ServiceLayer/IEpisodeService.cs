using DevPodcasts.ViewModels.Episode;

namespace DevPodcasts.ServiceLayer
{
    public interface IEpisodeService
    {
        EpisodeDetailViewModel GetEpisodeDetail(int id);

        bool EpisodeExists(int episodeId);
    }
}
