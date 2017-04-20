using DevPodcasts.Dtos;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Logging;
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

        public EpisodeUpdater(IPodcastRepository podcastRepository,
            IEpisodeRepository episodeRepository,
            IRssService rssService,
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
                var podcasts = _podcastRepository.GetDistinctPodcasts();
                await UpdatePodcasts(podcasts);
            });
        }

        private async Task UpdatePodcasts(IEnumerable<PodcastDto> podcasts)
        {
            var sw = Stopwatch.StartNew();
            foreach (var podcast in podcasts)
            {
                var newEpisodes = _rssService.GetNewEpisodes(podcast).ToList();
                foreach (var episode in newEpisodes)
                {
                    _logger.Info(podcast.Title + " \"" + episode.Title + "\" added");
                }
                await _episodeRepository.AddRange(newEpisodes);
            }
            sw.Stop();
            _logger.Info("Update took " + sw.ElapsedMilliseconds/1000 + " seconds");
        }
    }
}
