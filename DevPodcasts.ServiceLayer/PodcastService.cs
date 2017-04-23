using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ViewModels.Episode;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevPodcasts.DataLayer.Models;

namespace DevPodcasts.ServiceLayer
{
    public class PodcastService : IPodcastService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IRssService _rssService;
        private readonly IPodcastEmailService _podcastEmailService;
        private readonly ITagsRepository _tagsRepository;

        public PodcastService(
            IPodcastRepository podcastRepository,
            IEpisodeRepository episodeRepository,
            IRssService rssService,
            IPodcastEmailService podcastEmailService,
            ITagsRepository tagsRepository)
        {
            _podcastRepository = podcastRepository;
            _episodeRepository = episodeRepository;
            _rssService = rssService;
            _podcastEmailService = podcastEmailService;
            _tagsRepository = tagsRepository;
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
            var picks = _podcastRepository.GetFeaturedPodcasts(3);

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

        public PodcastDetailViewModel GetPodcastDetail(int podcastId)
        {
            var podcast = _podcastRepository.GetPodcast(podcastId);
            return new PodcastDetailViewModel
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description,
                SiteUrl = podcast.SiteUrl,
                ImageUrl = podcast.ImageUrl,
                Episodes = _episodeRepository.GetAllEpisodes(podcastId)
                    .Select(i => new EpisodeViewModel
                    {
                        Id = i.Id,
                        Title = i.Title,
                        DatePublished = i.DatePublished?.ToShortDateString()
                    })
            };
        }

        public bool PodcastExists(int podcastId)
        {
            return _podcastRepository.PodcastExists(podcastId);
        }

        public EditPodcastViewModel Edit(int podcastId)
        {
            var podcast = _podcastRepository.GetPodcastForEdit(podcastId);
            var viewModel = new EditPodcastViewModel
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description,
                ImageUrl = podcast.ImageUrl,
                FeedUrl = podcast.FeedUrl,
                SiteUrl = podcast.SiteUrl
            };

            var tags = _tagsRepository.GetAll();
            foreach (var tag in tags)
            {
                viewModel.Tags.Add(new CheckBoxListItem
                {
                    Id = tag.TagId,
                    Display = tag.Name
                });
            }

            return viewModel;
        }

        public void AddPodcastEpisodes(int podcastId)
        {
            _rssService.AddPodcastEpisodes(podcastId);
        }
    }
}
