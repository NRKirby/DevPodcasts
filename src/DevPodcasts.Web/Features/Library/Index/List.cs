using DevPodcasts.DataLayer.Models;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Library.Index
{
    public class List
    {
        public class Query : IRequest<ViewModel>
        {
            public string UserId { get; set; }
        }

        public class ViewModel : IRequest
        {
            public IEnumerable<SubscribedPodcast> SubscribedPodcasts { get; set; }
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

                var subscribedPodcasts = _context.LibraryPodcasts
                    .Where(user => user.UserId == message.UserId)
                    .OrderBy(podcast => podcast.PodcastTitle)
                    .Select(podcast => new SubscribedPodcast
                    {
                        Id = podcast.Id,
                        Title = podcast.PodcastTitle
                    }).ToList();

                viewModel.SubscribedPodcasts = subscribedPodcasts;
                viewModel.UserId = message.UserId;
                return viewModel;
            }
        }
    }

    public class SubscribedPodcast
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}