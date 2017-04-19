using DevPodcasts.ViewModels.Home;
using System.Linq;

namespace DevPodcasts.ServiceLayer
{
    public class HomeService : IHomeService
    {
        private readonly IPodcastService _podcastService;

        public HomeService(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public HomeIndexViewModel GetIndexViewModel()
        {
            return new HomeIndexViewModel
            {
                TotalPodcasts = _podcastService.GetTotalPodcasts(),
                PodcastPicks = _podcastService.GetPodcastPicks().ToArray()
            };
        }
    }
}
