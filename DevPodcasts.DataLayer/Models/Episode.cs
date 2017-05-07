using System;

namespace DevPodcasts.DataLayer.Models
{
    public class Episode : ModelBase<int>
    {
        public string EpisodeId { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string AudioUrl { get; set; }

        public string EpisodeUrl { get; set; }

        public DateTime? DatePublished { get; set; }

        public DateTime DateCreated { get; set; }

        public int PodcastId { get; set; }

        public virtual Podcast Podcast { get; set; }
    }
}
