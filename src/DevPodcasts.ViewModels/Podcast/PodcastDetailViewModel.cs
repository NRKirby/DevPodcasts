using DevPodcasts.ViewModels.Episode;
using DevPodcasts.ViewModels.Tags;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Podcast
{
    public class PodcastDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SiteUrl { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<EpisodeViewModel> Episodes { get; set; }
        public IEnumerable<TagViewModel> Tags { get; set; }
        public string UserId { get; set; }
    }
}
