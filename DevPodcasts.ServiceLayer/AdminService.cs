using DevPodcasts.DataLayer.Models;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class AdminService : IAdminService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IPodcastService _podcastService;

        public AdminService(IPodcastRepository podcastRepository, 
            ITagsRepository tagsRepository,
            IPodcastService podcastService)
        {
            _podcastRepository = podcastRepository;
            _tagsRepository = tagsRepository;
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

        public async Task Save(int podcastId, IEnumerable<int> selectedCategoryIds)
        {
            _podcastService.AddPodcastEpisodes(podcastId);
            await _podcastRepository.SaveCategories(podcastId, selectedCategoryIds);
        }

        public async Task Reject(int podcastId)
        {
            await _podcastRepository.Reject(podcastId);
        }

        public ReviewPodcastViewModel GetPodcastForReview(int podcastId)
        {
            var podcast = _podcastRepository.GetPodcast(podcastId);
            var tags = _tagsRepository.GetAll();
            var viewModel = new ReviewPodcastViewModel
            {
                Id = podcast.Id,
                Title = podcast.Title,
                SiteUrl = podcast.SiteUrl
            };

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

        public AdminManagePodcastsViewModel GetPodcastList()
        {
            var viewModel = new AdminManagePodcastsViewModel
            {
                Items = _podcastRepository
                    .GetAllPodcasts()
                    .Select(i => new AdminManagePodcastItemViewModel
                    {
                        Id = i.Id,
                        Title = i.Title
                    })
            };
            return viewModel;
        }
    }
}
