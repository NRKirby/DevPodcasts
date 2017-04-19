using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService : IPodcastService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IRssService _rssService;
        private readonly IPodcastEmailService _podcastEmailService;

        public PodcastService(
            IPodcastRepository podcastRepository, 
            IRssService rssService, 
            IPodcastEmailService podcastEmailService)
        {
            _podcastRepository = podcastRepository;
            _rssService = rssService;
            _podcastEmailService = podcastEmailService;
        }

        public int GetTotalPodcasts()
        {
            return _podcastRepository.GetTotalPodcasts();
        }

        public async Task<SubmitPodcastViewModel> SubmitPodcastForReview(SubmitPodcastViewModel model)
        {
            var podcastDto = _rssService.GetPodcastForReview(model.RssFeedUrl);
            if (podcastDto.SuccessResult == SuccessResult.Success)
            {
                await _podcastRepository.Add(podcastDto);
                await _podcastEmailService.SendPodcastSubmittedEmailAsync(podcastDto.Title);
            }

            var viewModel = new SubmitPodcastViewModel
            {
                RssFeedUrl = podcastDto.FeedUrl,
                SuccessResult = podcastDto.SuccessResult
            };

            return viewModel;
        }

        public IEnumerable<PodcastPick> GetPodcastPicks()
        {
            var picks = _podcastRepository.GetPodcastPicks();

            var podcastPicks = new List<PodcastPick>();

            foreach (var pick in picks)
            {
                podcastPicks.Add(new PodcastPick
                {
                    PodcastId = pick.Id,
                    Title = pick.Title,
                    Description = pick.Description
                });
            }

            return podcastPicks;
        }

        public void AddPodcastEpisodes(int podcastId)
        {
            _rssService.AddPodcastEpisodes(podcastId);
        }
    }
}
