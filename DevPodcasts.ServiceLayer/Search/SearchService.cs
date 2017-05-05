using System.Linq;
using DevPodcasts.Repositories;
using DevPodcasts.ViewModels.Search;

namespace DevPodcasts.ServiceLayer.Search
{
    public class SearchService : ISearchService
    {
        private readonly IPodcastRepository _podcastRepository; 

        public SearchService(IPodcastRepository podcastRepository)
        {
            _podcastRepository = podcastRepository;
        }

        public SearchIndexViewModel Search(string query, string type = "podcast")
        {
            var viewModel = new SearchIndexViewModel
            {
                PodcastSearchResults = _podcastRepository.Search(query)
                    .Select(i => new PodcastSearchResult
                    {
                        Id = i.Id,
                        Title = i.Title,
                        NumberOfEpisodes = i.NumberOfEpisodes,
                        Description = i.Description,
                        ImageUrl = i.ImageUrl
                    }).ToList()
            };
            return viewModel;
        }
    }
}
