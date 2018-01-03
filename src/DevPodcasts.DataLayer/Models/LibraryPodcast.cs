using System;

namespace DevPodcasts.DataLayer.Models
{
    public class LibraryPodcast
    {
        public int Id { get; set; }
        public int PodcastId { get; set; }
        public Podcast Podcast { get; set; }
        public string PodcastTitle { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
