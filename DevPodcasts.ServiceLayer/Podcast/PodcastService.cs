using DevPodcasts.DataLayer.Models;
using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ServiceLayer.RSS;
using DevPodcasts.ViewModels.Episode;
using DevPodcasts.ViewModels.Home;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Podcast
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

        public PodcastIndexViewModel Search(string query)
        {
            var podcasts = _podcastRepository.Search(query);
            var viewModel = new PodcastIndexViewModel
            {
                Items = podcasts
                .Select(i => new PodcastIndexItemViewModel
                {
                    Id = i.Id,
                    Title = i.Title,
                    ImageUrl = i.ImageUrl,
                    Description = i.Description,
                    NumberOfEpisodes = i.NumberOfEpisodes
                }),
                EpisodeCount = _episodeRepository.EpisodeCount()
            };

            return viewModel;
        }

        public IEnumerable<FeaturedPodcast> GetFeaturedPodcasts()
        {
            return _podcastRepository.GetFeaturedPodcasts(3);
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

        // TODO: rewrite this method
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

            var tags = _tagsRepository
                .GetAll()
                .ToList();

            foreach (var tag in tags)
            {
                viewModel.Tags.Add(new CheckBoxListItem
                {
                    Id = tag.TagId,
                    Display = tag.Name
                });
            }

            foreach (var tag in podcast.Tags)
            {
                var t = viewModel.Tags.Single(i => i.Id == tag.Id);
                t.IsChecked = true;
            }

            return viewModel;
        }

        public async Task UpdatePodcast(EditPodcastViewModel model)
        {
            var selectedTags = model
                .Tags
                .Where(i => i.IsChecked)
                .Select(i => new TagDto
            {
                Id = i.Id,
            });

            var dto = new PodcastDto
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                FeedUrl = model.FeedUrl,
                SiteUrl = model.SiteUrl,
                Tags = selectedTags
            };
            await _podcastRepository.UpdatePodcast(dto);
        }

        public void AddPodcastEpisodes(int podcastId)
        {
            Task.Run(() => _rssService.AddPodcastEpisodes(podcastId));
        }

        public HomeIndexViewModel GetHomePageViewModel()
        {
            var totalPodcasts = _podcastRepository.GetTotalPodcasts();
            var featured = _podcastRepository.GetFeaturedPodcasts(3);
            var recent = _episodeRepository.GetMostRecentEpisodes(10);

            return new HomeIndexViewModel
            {
                TotalPodcasts = totalPodcasts,
                FeaturedPodcasts = featured,
                RecentEpisodes = recent
            };
        }
    }
}
