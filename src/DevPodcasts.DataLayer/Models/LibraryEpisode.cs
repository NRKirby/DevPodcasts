using System;

namespace DevPodcasts.DataLayer.Models
{
    public class LibraryEpisode
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
