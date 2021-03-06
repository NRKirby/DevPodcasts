﻿using DevPodcasts.DataLayer.Models;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ViewModels.Admin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Admin
{
    public class AdminService
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly TagsRepository _tagsRepository;
        private readonly PodcastService _podcastService;

        public AdminService(PodcastRepository podcastRepository, 
            TagsRepository tagsRepository,
            PodcastService podcastService)
        {
            _podcastRepository = podcastRepository;
            _tagsRepository = tagsRepository;
            _podcastService = podcastService;
        }

        public AdminIndexViewModel GetIndexViewModel()
        {
            var viewModel = new AdminIndexViewModel
            {
                UnapprovedPodcasts = _podcastRepository.GetUnapprovedPodcasts()
            };
            return viewModel;
        }

        public async Task Save(ReviewPodcastViewModel model)
        {
            var podcastId = model.Id;
            var selectedTags = model.Tags.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            _podcastService.AddPodcastEpisodes(podcastId);
            await _podcastRepository.SaveTags(podcastId, selectedTags);
        }

        public async Task Reject(int podcastId)
        {
            await _podcastRepository.Reject(podcastId);
        }

        public ReviewPodcastViewModel GetPodcastForReview(int podcastId)
        {
            var podcast = _podcastRepository.GetPodcast(podcastId);
            var tags = _tagsRepository.GetAll();
            var viewModel = new ReviewPodcastViewModel
            {
                Id = podcast.Id,
                Title = podcast.Title,
                SiteUrl = podcast.SiteUrl
            };

            foreach (var tag in tags)
            {
                viewModel.Tags.Add(new CheckBoxListItem
                {
                    Id = tag.TagId,
                    Display = tag.Name
                });
            }

            return viewModel;
        }

        public AdminManagePodcastsViewModel GetPodcastList()
        {
            var viewModel = new AdminManagePodcastsViewModel
            {
                Items = _podcastRepository
                    .GetAllPodcasts()
                    .Select(i => new AdminManagePodcastItemViewModel
                    {
                        Id = i.Id,
                        Title = i.Title
                    })
            };
            return viewModel;
        }
    }
}
