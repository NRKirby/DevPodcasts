using DevPodcasts.DataLayer.Models;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Admin
{
    public class ReviewPodcastViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string SiteUrl { get; set; }

        public List<CheckBoxListItem> Tags { get; set; }

        public ReviewPodcastViewModel()
        {
            Tags = new List<CheckBoxListItem>();
        }
    }
}
