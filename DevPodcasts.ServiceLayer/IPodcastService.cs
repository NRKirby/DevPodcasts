using DevPodcasts.Dtos;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public interface IPodcastService
    {
        void AddPodcastEpisodes(int podcastId);

        int GetTotalPodcasts();

        Task<SubmitPodcastViewModel> SubmitPodcastForReview(SubmitPodcastViewModel model);

        IEnumerable<PodcastPick> GetPodcastPicks();
    }
}
