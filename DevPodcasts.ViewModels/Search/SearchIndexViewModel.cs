using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Search
{
    public class SearchIndexViewModel
    {
        public IEnumerable<PodcastSearchResult> PodcastSearchResults { get; set; }
    }

    public class PodcastSearchResult
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int NumberOfEpisodes { get; set; }
    }
}
