using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DevPodcasts.DataLayer.Models;
using DevPodcasts.Web.Features.Library;
using MediatR;

namespace DevPodcasts.Web.Features.Podcast
{
    public class AddOrRemove
    {
        public class Command : IRequest<AjaxModel>
        {
            public string UserId { get; set; }
            public int PodcastId { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, AjaxModel>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<AjaxModel> Handle(Command message)
            {
                var model = new AjaxModel { IsSuccess = false };
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == message.UserId);
                if (user == null)
                {
                    model.Error = "User doesn't exist";
                    return model;
                }

                var podcast = await _context.Podcasts.SingleOrDefaultAsync(p => p.Id == message.PodcastId);
                if (podcast == null)
                {
                    model.Error = "Podcast doesn't exist";
                    return model;
                }

                var userIsSubscribed = user.LibraryPodcasts.Any(p => p.PodcastId == podcast.Id);
                if (userIsSubscribed)
                {
                    var libraryPodcast = await _context.LibraryPodcasts.SingleAsync(p => p.PodcastId == podcast.Id);
                    _context.LibraryPodcasts.Remove(libraryPodcast);
                    user.LibraryPodcasts.Remove(libraryPodcast);
                    model.IsAdded = false;
                }
                else
                {
                    var libraryPodcast = new LibraryPodcast
                    {
                        PodcastId = podcast.Id,
                        Podcast = podcast,
                        UserId = user.Id,
                        ApplicationUser = user,
                        IsSubscribed = false,
                        DateAdded = DateTime.Now
                    };
                    user.LibraryPodcasts.Add(libraryPodcast);
                    _context.LibraryPodcasts.Add(libraryPodcast);
                    model.IsAdded = true;
                }

                model.IsSuccess = true;
                await _context.SaveChangesAsync();

                return model;
            }
        }
    }
}