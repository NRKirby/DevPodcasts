using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Search;

namespace DevPodcasts.ServiceLayer.Search
{
    public class SearchService
    {
        private readonly PodcastRepository _podcastRepository; 

        public SearchService(PodcastRepository podcastRepository)
        {
            _podcastRepository = podcastRepository;
        }

        public SearchResultsViewModel Search(string query, string type = "podcast")
        {
            var viewModel = new SearchResultsViewModel
            {
                PodcastSearchResults = _podcastRepository.Search(query)
            };
            return viewModel;
        }
    }
}
