using DevPodcasts.ViewModels.Admin;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public interface IAdminService
    {
        AdminIndexViewModel GetIndexViewModel();

        void Approve(int podcastId);

        Task Reject(int podcastId);

        ReviewPodcastViewModel GetPodcastForReview(int podcastId);
    }
}
