using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Tags
{
    public class TagResultViewModel
    {
        public string TagName { get; set; }

        public IEnumerable<PodcastSearchResultViewModel> SearchResults { get; set; }
    }
}
