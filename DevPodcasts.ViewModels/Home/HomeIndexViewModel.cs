using DevPodcasts.Dtos;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public int TotalPodcasts { get; set; }

        public IEnumerable<PodcastPick> PodcastPicks { get; set; }
    }
}
