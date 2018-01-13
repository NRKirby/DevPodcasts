using DevPodcasts.DataLayer.Models;
using DevPodcasts.ViewModels.Episode;
using MediatR;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Episode
{
    public class Detail
    {
        public class Query : IRequest<EpisodeDetailViewModel>
        {
            public string UserId { get; set; }
            public int EpisodeId { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, EpisodeDetailViewModel>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<EpisodeDetailViewModel> Handle(Query message)
            {
                var episodeId = message.EpisodeId;
                var userId = message.UserId;
                var episode = await _context.Episodes
                    .SingleOrDefaultAsync(e => e.Id == episodeId);

                var isSubscribed = false;
                if (userId != null)
                {
                    isSubscribed = _context.Users
                        .Single(u => u.Id == userId)
                        .LibraryEpisodes
                        .Any(e => e.EpisodeId == episodeId);
                }

                var viewModel = new EpisodeDetailViewModel
                {
                    Id = episode.Id,
                    PodcastId = episode.PodcastId,
                    Title = episode.Title,
                    AudioUrl = episode.AudioUrl,
                    EpisodeUrl = episode.EpisodeUrl,
                    PodcastTitle = episode.Podcast.Title,
                    Summary = episode.Summary,
                    IsSubscribed = isSubscribed,
                    UserId = userId
                };

                if (episode.DatePublished != null)
                {
                    viewModel.DatePublished = (DateTime)episode.DatePublished;
                }

                return viewModel;
            }
        }
    }
}