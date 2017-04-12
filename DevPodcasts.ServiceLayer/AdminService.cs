using System.Threading.Tasks;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Admin;

namespace DevPodcasts.ServiceLayer
{
    public class AdminService
    {
        private readonly PodcastRepository _repository;

        public AdminService()
        {
            _repository = new PodcastRepository();
        }

        public AdminIndexViewModel GetIndexViewModel()
        {
            var viewModel = new AdminIndexViewModel
            {
                UnapprovedPodcasts = _repository.GetUnapprovedPodcasts()
            };
            return viewModel;
        }

        public void Approve(int podcastId)
        {
            
        }

        public async Task Reject(int podcastId)
        {
            await _repository.Reject(podcastId);
        }
    }
}
