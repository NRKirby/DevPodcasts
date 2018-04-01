using System;

namespace DevPodcasts.Models.Dtos
{
    public class EpisodeDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string AudioUrl { get; set; }

        public string EpisodeUrl { get; set; }

        public int PodcastId { get; set; }

        public string PodcastTitle { get; set; }    

        public string EpisodeId { get; set; }

        public DateTime? DatePublished { get; set; }

        public DateTime DateCreated { get; set; }       
    }
}