﻿using DevPodcasts.DataLayer.Models;
using DevPodcasts.Models.DTOs;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.RSS;
using DevPodcasts.ViewModels.Home;
using DevPodcasts.ViewModels.Podcast;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Podcast
{
    public class PodcastService
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly EpisodeRepository _episodeRepository;
        private readonly RssService _rssService;
        private readonly TagsRepository _tagsRepository;

        public PodcastService(
            PodcastRepository podcastRepository,
            EpisodeRepository episodeRepository,
            RssService rssService,
            TagsRepository tagsRepository)
        {
            _podcastRepository = podcastRepository;
            _episodeRepository = episodeRepository;
            _rssService = rssService;
            _tagsRepository = tagsRepository;
        }

        public PodcastIndexViewModel Search(string query = null)
        {
            var viewModel = new PodcastIndexViewModel
            {
                Items = _podcastRepository.Search(query),
                EpisodeCount = _episodeRepository.EpisodeCount()
            };

            return viewModel;
        }

        public IEnumerable<FeaturedPodcast> GetFeaturedPodcasts()
        {
            return _podcastRepository.GetFeaturedPodcasts(3);
        }

        public bool PodcastExists(int podcastId)
        {
            return _podcastRepository.PodcastExists(podcastId);
        }

        // TODO: rewrite this method
        public EditPodcastViewModel Edit(int podcastId) 
        {
            var podcast = _podcastRepository.GetPodcastForEdit(podcastId);
            var viewModel = new EditPodcastViewModel
            {
                Id = podcast.Id,
                Title = podcast.Title,
                Description = podcast.Description,
                ImageUrl = podcast.ImageUrl,
                FeedUrl = podcast.FeedUrl,
                SiteUrl = podcast.SiteUrl
            };

            var tags = _tagsRepository
                .GetAll()
                .ToList();

            foreach (var tag in tags)
            {
                viewModel.Tags.Add(new CheckBoxListItem
                {
                    Id = tag.TagId,
                    Display = tag.Name
                });
            }

            foreach (var tag in podcast.Tags)
            {
                var t = viewModel.Tags.Single(i => i.Id == tag.Id);
                t.IsChecked = true;
            }

            return viewModel;
        }

        public async Task UpdatePodcast(EditPodcastViewModel model)
        {
            var selectedTags = model
                .Tags
                .Where(i => i.IsChecked)
                .Select(i => new TagDto
            {
                Id = i.Id,
            });

            var dto = new PodcastDto
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                FeedUrl = model.FeedUrl,
                SiteUrl = model.SiteUrl,
                Tags = selectedTags
            };
            await _podcastRepository.UpdatePodcast(dto);
        }

        public void AddPodcastEpisodes(int podcastId)
        {
            Task.Run(() => _rssService.AddPodcastEpisodes(podcastId));
        }

        public HomeIndexViewModel GetHomePageViewModel()
        {
            var totalEpisodes = _episodeRepository.EpisodeCount();
            var totalPodcasts = _podcastRepository.GetTotalPodcasts();
            var recentEpisodes = _episodeRepository.GetMostRecentEpisodes(10);
            var featuredPodcasts = _podcastRepository.GetFeaturedPodcasts(5);

            return new HomeIndexViewModel
            {
                TotalEpisodes = totalEpisodes,
                TotalPodcasts = totalPodcasts,
                FeaturedPodcasts = featuredPodcasts,
                RecentEpisodes = recentEpisodes
            };
        }

        public async Task DeletePodcast(int podcastId)
        {
            await _podcastRepository.DeletePodcast(podcastId);
        }
    }
}
