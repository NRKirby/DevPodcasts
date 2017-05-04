using DevPodcasts.Dtos;
using DevPodcasts.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public interface IEpisodeRepository
    {
        Task AddRange(IEnumerable<EpisodeDto> dtos);

        IEnumerable<EpisodeDto> GetAllEpisodes(int podcastId);

        IEnumerable<RecentEpisode> GetMostRecentEpisodes(int numberOfEpisodes);

        EpisodeDto GetEpisode(int episodeId);

        DateTime? GetMostRecentEpisodeDate(int podcastId);

        bool EpisodeExists(int episodeId);

        int EpisodeCount();
    }
}
