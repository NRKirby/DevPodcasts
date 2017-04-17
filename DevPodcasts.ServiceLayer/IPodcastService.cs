﻿using DevPodcasts.ViewModels.Podcast;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public interface IPodcastService
    {
        void AddPodcastEpisodes(int podcastId);

        Task<SubmitPodcastViewModel> SubmitPodcastForReview(SubmitPodcastViewModel model);
    }
}
