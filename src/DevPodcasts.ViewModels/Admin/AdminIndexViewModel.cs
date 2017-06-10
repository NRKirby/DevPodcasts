using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Admin
{
    public class AdminIndexViewModel
    {
        public IEnumerable<PodcastViewModel> UnapprovedPodcasts { get; set; }
    }
}
