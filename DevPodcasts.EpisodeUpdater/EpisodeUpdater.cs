﻿using DevPodcasts.Dtos;
using DevPodcasts.Logging;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.RSS;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.EpisodeUpdater
{
    public class EpisodeUpdater
    {
        private readonly PodcastRepository _podcastRepository;
        private readonly EpisodeRepository _episodeRepository;
        private readonly RssService _rssService;
        private readonly ILogger _logger;
        private int _episodesAddedCount;

        public EpisodeUpdater(PodcastRepository podcastRepository,
            EpisodeRepository episodeRepository,
            RssService rssService,
            ILogger logger)
        {
            _podcastRepository = podcastRepository;
            _episodeRepository = episodeRepository;
            _rssService = rssService;
            _logger = logger;
        }

        public void Update()
        {
            Task.Run(async () =>
            {
                var sw = Stopwatch.StartNew();
                var podcasts = _podcastRepository.GetDistinctPodcasts();
                await UpdatePodcasts(podcasts);
                sw.Stop();
                if (_episodesAddedCount > 0)
                    _logger.Info($"{_episodesAddedCount} episodes added");
                _logger.Debug($"Update took {sw.ElapsedMilliseconds / 1000} seconds");
            });
        }

        private async Task UpdatePodcasts(IEnumerable<PodcastDto> podcasts)
        {
            _episodesAddedCount = 0;
            var podcastList = podcasts.ToList();
            _logger.Debug($"Begin update of {podcastList.Count} podcasts");
            foreach (var podcast in podcastList)
            {
                var newEpisodes = _rssService.GetNewEpisodes(podcast).ToList();

                var addedCount = await _episodeRepository.AddRange(newEpisodes);
                _episodesAddedCount += addedCount;
            }
        }

        public void UpdateSync()
        {
            var sw = Stopwatch.StartNew();
            var podcasts = _podcastRepository.GetDistinctPodcasts();
            UpdatePodcastsSync(podcasts);
            sw.Stop();
            if (_episodesAddedCount > 0)
                _logger.Info($"{_episodesAddedCount} episodes added");
            _logger.Debug($"Update took {sw.ElapsedMilliseconds / 1000} seconds");
        }

        private void UpdatePodcastsSync(IEnumerable<PodcastDto> podcasts)
        {
            _episodesAddedCount = 0;
            var podcastList = podcasts.ToList();
            _logger.Debug($"Begin update of {podcastList.Count} podcasts");
            foreach (var podcast in podcastList)
            {
                var newEpisodes = _rssService.GetNewEpisodes(podcast).ToList();

                var addedCount = _episodeRepository.AddRangeSync(newEpisodes);
                _episodesAddedCount += addedCount;
            }
        }
    }
}
