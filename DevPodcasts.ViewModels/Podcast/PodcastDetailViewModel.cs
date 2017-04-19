using DevPodcasts.ViewModels.Episode;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Podcast
{
    public class PodcastDetailViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SiteUrl { get; set; }

        public IEnumerable<EpisodeViewModel> Episodes { get; set; }
    }
}
