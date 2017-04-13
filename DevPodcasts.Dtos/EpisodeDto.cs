using System;

namespace DevPodcasts.Dtos
{
    public class EpisodeDto
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public string AudioUrl { get; set; }

        public string EpisodeUrl { get; set; }

        public int PodcastId { get; set; }

        public string EpisodeId { get; set; }

        public DateTime DatePublished { get; set; }

        public DateTime DateCreated { get; set; }       
    }
}