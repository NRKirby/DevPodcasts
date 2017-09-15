using DevPodcasts.ViewModels.Tags;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Home
{
    public class FeaturedPodcast
    {
        public int PodcastId { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; }
    }
}
