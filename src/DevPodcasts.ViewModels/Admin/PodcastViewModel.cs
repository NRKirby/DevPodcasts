using System;

namespace DevPodcasts.ViewModels.Admin
{
    public class PodcastViewModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Title { get; set; }

        public string SiteUrl { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
