using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Podcast;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService
    {
        private readonly PodcastRepository _repository;
        private readonly RssService _service;

        public PodcastService()
        {
            _repository = new PodcastRepository();
            _service = new RssService();
        }

        public async Task<AddPodcastViewModel> AddPodcastForReview(AddPodcastViewModel model)
        {
            var podcastDto = _service.GetPodcastForReview(model.RssFeedUrl);
            if (podcastDto.SuccessResult == SuccessResult.Success)
            {
                await _repository.Add(podcastDto);
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
            _service.AddPodcastEpisodes(podcastId);
        }
    }
}
