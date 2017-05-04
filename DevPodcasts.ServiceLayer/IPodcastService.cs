using DevPodcasts.ViewModels.Home;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public interface IPodcastService
    {
        void AddPodcastEpisodes(int podcastId);

        int GetTotalPodcasts();

        HomeIndexViewModel GetHomePageViewModel();

        Task<SubmitPodcastViewModel> SubmitPodcastForReview(SubmitPodcastViewModel model);

        PodcastIndexViewModel Search(string query = null);

        IEnumerable<FeaturedPodcast> GetFeaturedPodcasts();

        PodcastDetailViewModel GetPodcastDetail(int podcastId);

        bool PodcastExists(int podcastId);

        EditPodcastViewModel Edit(int podcastId);

        Task UpdatePodcast(EditPodcastViewModel model);
    }
}
