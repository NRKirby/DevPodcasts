using System;

namespace DevPodcasts.DataLayer.Models
{
    public class ListenLater : ModelBase<int>
    {
        public string UserId { get; set; }

        public int EpisodeId { get; set; }

        public DateTime AddedTimeStamp { get; set; }
    }
}
