using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Admin
{
    public class AdminManagePodcastsViewModel
    {
        public IEnumerable<AdminManagePodcastItemViewModel> Items { get; set; }
    }
}
