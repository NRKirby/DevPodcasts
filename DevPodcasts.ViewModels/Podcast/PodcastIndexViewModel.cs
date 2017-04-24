using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Podcast
{
    public class PodcastIndexViewModel
    {
        public IEnumerable<PodcastIndexItemViewModel> Items { get; set; }

        public int EpisodeCount { get; set; }
    }
}
