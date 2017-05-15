using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ViewModels.Home;

namespace DevPodcasts.ServiceLayer.Home
{
    public class HomeService : IHomeService
    {
        private readonly PodcastService _podcastService;

        public HomeService(PodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public HomeIndexViewModel GetIndexViewModel()
        {
            return _podcastService.GetHomePageViewModel();
        }
    }
}
