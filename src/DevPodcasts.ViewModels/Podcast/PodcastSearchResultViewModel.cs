using DevPodcasts.ViewModels.Tags;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Podcast
{
    public class PodcastSearchResultViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int NumberOfEpisodes { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}
