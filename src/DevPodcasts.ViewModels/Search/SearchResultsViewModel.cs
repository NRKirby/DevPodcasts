using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Search
{
    public class SearchResultsViewModel
    {
        public IEnumerable<PodcastSearchResultViewModel> PodcastSearchResults { get; set; }
    }
}
