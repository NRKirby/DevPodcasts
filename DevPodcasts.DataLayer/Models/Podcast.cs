using System;
namespace DevPodcasts.DataLayer.Models
{
    public class Podcast
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string FeedUrl { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool? IsApproved { get; set; }

        public DateTime? DateApproved { get; set; }
    }
}
