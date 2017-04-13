using DevPodcasts.Dtos;
using System.Collections.Generic;

namespace DevPodcasts.ServiceLayer
{
    public interface IRssParser
    {
        IEnumerable<EpisodeDto> GetNewEpisodes(PodcastDto podcastDto);

        PodcastDto GetPodcastForReview(string rssFeedUrl);

        void AddPodcastEpisodes(int podcastId);
    }
}
