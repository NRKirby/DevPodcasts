using DevPodcasts.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DevPodcasts.ViewModels.Podcast
{
    public class EditPodcastViewModel
    {
        public EditPodcastViewModel()
        {
            Tags = new List<CheckBoxListItem>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        [DisplayName("Feed Url")]
        public string FeedUrl { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Approved?")]
        public bool? IsApproved { get; set; }

        [DisplayName("Date Approved")]
        public DateTime? DateApproved { get; set; }

        [DisplayName("Site Url")]
        public string SiteUrl { get; set; }

        public ICollection<CheckBoxListItem> Tags { get; set; }
    }
}
