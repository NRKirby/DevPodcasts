using DevPodcasts.DataLayer.Models;
using DevPodcasts.ViewModels.Episode;
using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.ViewModels.Tags;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Podcast
{
    public class List
    {
        public class Query : IRequest<PodcastDetailViewModel>
        {
            public string UserId { get; set; }
            public int PodcastId { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, PodcastDetailViewModel>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PodcastDetailViewModel> Handle(Query message)
            {
                var podcastId = message.PodcastId;
                var userId = message.UserId;
                var podcast = await _context.Podcasts
                    .SingleOrDefaultAsync(p => p.Id == podcastId);

                var episodes = await _context.Episodes
                    .AsNoTracking()
                    .Where(episode => episode.PodcastId == podcastId)
                    .OrderByDescending(episode => episode.DatePublished)
                    .Select(episode => new { episode.Id, episode.Title, episode.DatePublished })
                    .ToListAsync();

                var isSubscribed = false;
                if (message.UserId != null)
                {
                    isSubscribed = _context.Users
                        .Single(u => u.Id == userId)
                        .LibraryPodcasts
                        .Any(p => p.PodcastId == podcastId);
                }

                var viewModel = new PodcastDetailViewModel
                {
                    Id = podcast.Id,
                    Title = podcast.Title,
                    Description = podcast.Description,
                    SiteUrl = podcast.SiteUrl,
                    ImageUrl = podcast.ImageUrl,
                    Tags = podcast.Tags
                        .Select(tag => new TagViewModel { Name = tag.Name, Slug = tag.Slug }),
                    IsSubscribed = isSubscribed,
                    UserId = userId,
                };

                var episodeList = new List<EpisodeViewModel>();
                foreach (var episode in episodes)
                {
                    var episodeViewModel = new EpisodeViewModel
                    {
                        Id = episode.Id,
                        Title = episode.Title
                    };

                    if (episode.DatePublished != null)
                        episodeViewModel.DatePublished = (DateTime)episode.DatePublished;

                    episodeList.Add(episodeViewModel);
                }

                viewModel.Episodes = episodeList;

                return viewModel;
            }
        }
    }
}