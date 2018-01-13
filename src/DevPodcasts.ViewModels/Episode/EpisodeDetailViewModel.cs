using System;

namespace DevPodcasts.ViewModels.Episode
{
    public class EpisodeDetailViewModel
    {
        public int Id { get; set; }
        public int PodcastId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string AudioUrl { get; set; }
        public string EpisodeUrl { get; set; }
        public string PodcastTitle { get; set; }
        public DateTime DatePublished { get; set; }
        public string UserId { get; set; }
        public bool IsSubscribed { get; set; }
    }
}
