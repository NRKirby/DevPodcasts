using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Logging;
using DevPodcasts.ServiceLayer.RSS;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer
{
    public class EpisodeUpdater
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly IRssService _rssService;
        private readonly ILogger _logger;
        private int EpisodesAddedCount;

        public EpisodeUpdater(IPodcastRepository podcastRepository,
            IEpisodeRepository episodeRepository,
            IRssService rssService,
            ILogger logger)
        {
            _podcastRepository = podcastRepository;
            _episodeRepository = episodeRepository;
            _rssService = rssService;
            _logger = logger;
            EpisodesAddedCount = 0;
        }

        public void Update()
        {
            Task.Run(async () =>
            {
                var sw = Stopwatch.StartNew();
                var podcasts = _podcastRepository.GetDistinctPodcasts();
                await UpdatePodcasts(podcasts);
                sw.Stop();
                _logger.Info(EpisodesAddedCount + " episodes added");
                _logger.Debug("Update took " + sw.ElapsedMilliseconds / 1000 + " seconds");
            });
        }

        private async Task UpdatePodcasts(IEnumerable<PodcastDto> podcasts)
        {
            var podcastList = podcasts.ToList();
            _logger.Debug("Total number podcasts: " + podcastList.Count);
            //var count = 1;
            foreach (var podcast in podcastList)
            {
                var newEpisodes = _rssService.GetNewEpisodes(podcast).ToList();                   
                foreach (var episode in newEpisodes)
                {
                    _logger.Info(podcast.Title + " \"" + episode.Title + "\" added");
                    EpisodesAddedCount++;
                }
                await _episodeRepository.AddRange(newEpisodes);
                //_logger.Debug("Podcast updater count: " + count);
                //count++;
            }
        }
    }
}
