using DevPodcasts.DataLayer.Models;
using MediatR;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Library
{
    public class AddOrRemovePodcast
    {
        public class Command : IRequest<AddRemoveLibraryAjaxModel>
        {
            public string UserId { get; set; }
            public int PodcastId { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, AddRemoveLibraryAjaxModel>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<AddRemoveLibraryAjaxModel> Handle(Command message)
            {
                var model = new AddRemoveLibraryAjaxModel { IsSuccess = false };
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
                        PodcastTitle = podcast.Title,
                        UserId = user.Id,
                        ApplicationUser = user,
                        IsSubscribed = true,
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