﻿using DevPodcasts.ViewModels.Episode;
using System.Collections.Generic;

namespace DevPodcasts.ViewModels.Podcast
{
    public class PodcastDetailViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SiteUrl { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<EpisodeViewModel> Episodes { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; }
    }

    public class TagViewModel
    {
        public string Name { get; set; }

        public string Slug { get; set; }
    }
}
