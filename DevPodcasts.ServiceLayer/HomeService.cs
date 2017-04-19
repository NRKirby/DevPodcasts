﻿using System.Linq;
using DevPodcasts.ViewModels.Home;

namespace DevPodcasts.ServiceLayer
{
    public class HomeService : IHomeService
    {
        private readonly IPodcastService _podcastService;

        public HomeService(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public HomeIndexViewModel GetIndexViewModel()
        {
            return new HomeIndexViewModel
            {
                TotalPodcasts = _podcastService.GetTotalPodcasts(),
                PodcastPicks = _podcastService.GetPodcastPicks().ToArray()
            };
        }
    }
}
