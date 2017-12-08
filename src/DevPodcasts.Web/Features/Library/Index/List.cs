using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DevPodcasts.DataLayer.Models;

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
                var subscribedPodcasts = _context.Users
                    .Single(user => user.Id == message.UserId)
                    .SubscribedPodcasts
                    .OrderBy(podcast => podcast.Title)
                    .Select(podcast => new SubscribedPodcast
                    {
                        Id = podcast.Id,
                        Title = podcast.Title
                    }).ToList();

                viewModel.SubscribedPodcasts = subscribedPodcasts;
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