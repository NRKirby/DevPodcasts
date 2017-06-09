using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Podcast
{
    public class PodcastIndexViewModel
    {
        public IEnumerable<PodcastSearchResultItemViewModel> Items { get; set; }

        public int EpisodeCount { get; set; }
    }
}
