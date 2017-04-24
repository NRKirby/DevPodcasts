using System;
using System.Collections.Generic;

namespace DevPodcasts.DataLayer.Models
{
    public class Podcast
    {
        public Podcast()
        {
            Episodes = new List<Episode>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string FeedUrl { get; set; }

        public DateTime DateCreated { get; set; }

        public bool? IsApproved { get; set; }

        public DateTime? DateApproved { get; set; }

        public string SiteUrl { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<ApplicationUser> SubscribedUsers { get; set; }
    }
}
