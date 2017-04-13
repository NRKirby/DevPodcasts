using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Admin;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class AdminService
    {
        private readonly PodcastRepository _repository;
        private readonly PodcastService _podcastService;

        public AdminService()
        {
            _repository = new PodcastRepository();
            _podcastService = new PodcastService();
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
            _podcastService.AddPodcastEpisodes(podcastId);
        }

        public async Task Reject(int podcastId)
        {
            await _repository.Reject(podcastId);
        }
    }
}
