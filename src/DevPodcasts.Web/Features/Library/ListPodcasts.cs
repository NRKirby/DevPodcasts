﻿using DevPodcasts.DataLayer.Models;
using MediatR;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Library
{
    public class ListPodcasts
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

                var subscribedPodcasts = await _context.LibraryPodcasts
                    .Where(user => user.UserId == message.UserId)
                    .OrderBy(podcast => podcast.PodcastTitle)
                    .Select(podcast => new SubscribedPodcast
                    {
                        PodcastId = podcast.PodcastId,
                        PodcastTitle = podcast.PodcastTitle,
                        ReceiveEmailAlerts = podcast.IsSubscribed
                    }).ToListAsync();

                viewModel.SubscribedPodcasts = subscribedPodcasts;
                viewModel.UserId = message.UserId;
                return viewModel;
            }
        }

        public class ViewModel : IRequest
        {
            public IEnumerable<SubscribedPodcast> SubscribedPodcasts { get; set; }
            public string UserId { get; set; }
        }

        public class SubscribedPodcast
        {
            public int PodcastId { get; set; }
            public string PodcastTitle { get; set; }
            public bool ReceiveEmailAlerts { get; set; }
        }
    }
}