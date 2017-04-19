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

        int GetTotalPodcasts();

        IEnumerable<PodcastViewModel> GetUnapprovedPodcasts();

        IEnumerable<PodcastDto> GetFeaturedPodcasts(int numberOfPodcasts);

        bool PodcastExists(string rssFeedUrl);

        Task Reject(int podcastId);

        Task SaveCategories(int podcastId, IEnumerable<int> categoryIds);

        bool PodcastExists(int podcastId);
    }
}
