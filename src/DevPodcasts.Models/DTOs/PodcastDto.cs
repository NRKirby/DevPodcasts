using DevPodcasts.Models.Dtos;
using System.Collections.Generic;

namespace DevPodcasts.Models.DTOs
{
    public class PodcastDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string FeedUrl { get; set; }

        public string SiteUrl { get; set; }

        public SuccessResult SuccessResult { get; set; }

        public IEnumerable<EpisodeDto> Episodes { get; set; }

        public IEnumerable<TagDto> Tags { get; set; }
    }
}
