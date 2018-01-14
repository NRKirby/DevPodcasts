using DevPodcasts.DataLayer.Models;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Library
{
    public class ListEpisodes
    {
        public class Query : IRequest<ViewModel>
        {
            public string UserId { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, ViewModel>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<ViewModel> Handle(Query message)
            {
                var viewModel = new ViewModel();

                var subscribedEpisodes = await _context.LibraryEpisodes
                    .Where(user => user.UserId == message.UserId)
                    .OrderBy(episode => episode.Episode.Title)
                    .Select(episode => new SubscribedEpisode
                    {
                        Id = episode.EpisodeId,
                        Title = episode.Episode.Title,
                        PodcastTitle = episode.Episode.Podcast.Title
                    }).ToListAsync();

                viewModel.SubscribedEpisodes = subscribedEpisodes;
                viewModel.UserId = message.UserId;
                return viewModel;
            }
        }

        public class ViewModel
        {
            public IEnumerable<SubscribedEpisode> SubscribedEpisodes { get; set; }
            public string UserId { get; set; }
        }

        public class SubscribedEpisode
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string PodcastTitle { get; set; }
        }
    }
}