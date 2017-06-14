using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public int TotalPodcasts { get; set; }

        public int TotalEpisodes { get; set; }

        public IEnumerable<FeaturedPodcast> FeaturedPodcasts { get; set; }

        public IEnumerable<RecentEpisode> RecentEpisodes { get; set; }
    }
}
