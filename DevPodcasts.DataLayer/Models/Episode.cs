using System;

namespace DevPodcasts.DataLayer.Models
{
    public class Episode
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string AudioUrl { get; set; }

        public DateTime? DatePublished { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
