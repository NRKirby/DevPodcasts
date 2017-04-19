﻿using DevPodcasts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPodcasts.Repositories
{
    public interface IEpisodeRepository
    {
        Task AddRange(IEnumerable<EpisodeDto> dtos);

        IEnumerable<EpisodeDto> GetAllEpisodes(int podcastId);

        DateTime? GetMostRecentEpisodeDate(int podcastId);
    }
}
