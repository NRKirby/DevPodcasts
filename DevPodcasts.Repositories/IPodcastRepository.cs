using DevPodcasts.Dtos;
using DevPodcasts.ViewModels.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public interface IPodcastRepository
    {
        Task Add(PodcastDto dto);

        void AddEpisodesToPodcast(PodcastDto dto);

        IEnumerable<PodcastDto> GetDistinctPodcasts();

        PodcastDto GetPodcast(int podcastId);

        IEnumerable<PodcastViewModel> GetUnapprovedPodcasts();

        bool PodcastExists(string rssFeedUrl);

        Task Reject(int podcastId);
    }
}
