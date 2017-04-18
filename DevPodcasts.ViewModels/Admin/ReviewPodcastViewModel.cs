using DevPodcasts.DataLayer.Models;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Admin
{
    public class ReviewPodcastViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SiteUrl { get; set; }

        public List<CheckBoxListItem> Categories { get; set; }

        public ReviewPodcastViewModel()
        {
            Categories = new List<CheckBoxListItem>();
        }
    }
}
