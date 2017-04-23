using DevPodcasts.DataLayer.Models;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class AdminService : IAdminService
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IPodcastService _podcastService;

        public AdminService(IPodcastRepository podcastRepository, 
            ICategoriesRepository categoriesRepository,
            IPodcastService podcastService)
        {
            _podcastRepository = podcastRepository;
            _categoriesRepository = categoriesRepository;
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
            var categories = _categoriesRepository.GetAll();
            var viewModel = new ReviewPodcastViewModel
            {
                Id = podcast.Id,
                Title = podcast.Title,
                SiteUrl = podcast.SiteUrl
            };

            foreach (var category in categories)
            {
                viewModel.Categories.Add(new CheckBoxListItem
                {
                    Id = category.TagId,
                    Display = category.Name
                });
            }

            return viewModel;
        }
    }
}
