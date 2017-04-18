using System.Collections.Generic;

namespace DevPodcasts.DataLayer.Models
{
    public class Category
    {
        public Category()
        {
            Podcasts = new List<Podcast>();
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Podcast> Podcasts { get; set; }
    }
}
