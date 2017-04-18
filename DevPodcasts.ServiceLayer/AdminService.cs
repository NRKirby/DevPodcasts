using System.Runtime.Remoting.Messaging;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Admin;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class AdminService : IAdminService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IPodcastService _podcastService;

        public AdminService(IPodcastRepository podcastRepository, IPodcastService podcastService)
        {
            _podcastRepository = podcastRepository;
            _podcastService = podcastService;
        }

        public AdminIndexViewModel GetIndexViewModel()
        {
            var viewModel = new AdminIndexViewModel
            {
                UnapprovedPodcasts = _podcastRepository.GetUnapprovedPodcasts()
            };
            return viewModel;
        }

        public void Approve(int podcastId)
        {
            _podcastService.AddPodcastEpisodes(podcastId);
        }

        public async Task Reject(int podcastId)
        {
            await _podcastRepository.Reject(podcastId);
        }

        public ReviewPodcastViewModel GetPodcastForReview(int podcastId)
        {
            var podcast = _podcastRepository.GetPodcast(podcastId);
            return new ReviewPodcastViewModel
            {
                Title = podcast.Title,
                SiteUrl = podcast.SiteUrl
            };
        }
    }
}
