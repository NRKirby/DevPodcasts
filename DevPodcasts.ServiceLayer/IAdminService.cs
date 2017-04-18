using DevPodcasts.ViewModels.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public interface IAdminService
    {
        AdminIndexViewModel GetIndexViewModel();

        Task Save(int podcastId, IEnumerable<int> selectedCategoryIds);

        Task Reject(int podcastId);

        ReviewPodcastViewModel GetPodcastForReview(int podcastId);
    }
}
