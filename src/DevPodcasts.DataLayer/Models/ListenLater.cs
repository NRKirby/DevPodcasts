using System;

namespace DevPodcasts.DataLayer.Models
{
    public class ListenLater
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int EpisodeId { get; set; }

        public virtual Episode Episode { get; set; }

        public DateTime AddedTimeStamp { get; set; }
    }
}
