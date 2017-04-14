using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Podcast;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService : IPodcastService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IRssService _rssService;

        public PodcastService(IPodcastRepository podcastRepository, IRssService rssService)
        {
            _podcastRepository = podcastRepository;
            _rssService = rssService;
        }

        public async Task<AddPodcastViewModel> AddPodcastForReview(AddPodcastViewModel model)
        {
            var podcastDto = _rssService.GetPodcastForReview(model.RssFeedUrl);
            if (podcastDto.SuccessResult == SuccessResult.Success)
            {
                await _podcastRepository.Add(podcastDto);
            }

            var viewModel = new AddPodcastViewModel
            {
                RssFeedUrl = podcastDto.FeedUrl,
                SuccessResult = podcastDto.SuccessResult
            };

            return viewModel;
        }
        
        public void AddPodcastEpisodes(int podcastId)
        {
            _rssService.AddPodcastEpisodes(podcastId);
        }
    }
}
