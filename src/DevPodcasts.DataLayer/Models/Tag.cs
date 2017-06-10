using System.Collections.Generic;

namespace DevPodcasts.DataLayer.Models
{
    public class Tag
    {
        public Tag()
        {
            Podcasts = new List<Podcast>();
        }

        public int TagId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public virtual ICollection<Podcast> Podcasts { get; set; }
    }
}
