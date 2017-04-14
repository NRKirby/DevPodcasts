using DevPodcasts.Dtos;
using System.Collections.Generic;

namespace DevPodcasts.ServiceLayer
{
    public interface IRssService
    {
        void AddPodcastEpisodes(int podcastId);

        IEnumerable<EpisodeDto> GetNewEpisodes(PodcastDto podcastDto);

        PodcastDto GetPodcastForReview(string rssFeedUrl);
    }
}
