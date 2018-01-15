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

                var bookmarkedEpisodes = await _context.LibraryEpisodes
                    .Where(user => user.UserId == message.UserId)
                    .OrderByDescending(episode => episode.DateAdded)
                    .Select(episode => new BookmarkedEpisode
                    {
                        EpisodeId = episode.EpisodeId,
                        EpisodeTitle = episode.Episode.Title,
                        PodcastTitle = episode.Episode.Podcast.Title
                    }).ToListAsync();

                viewModel.BookmarkedEpisodes = bookmarkedEpisodes;
                viewModel.UserId = message.UserId;
                return viewModel;
            }
        }

        public class ViewModel
        {
            public IEnumerable<BookmarkedEpisode> BookmarkedEpisodes { get; set; }
            public string UserId { get; set; }
        }

        public class BookmarkedEpisode
        {
            public int EpisodeId { get; set; }
            public string EpisodeTitle { get; set; }
            public string PodcastTitle { get; set; }
        }
    }
}