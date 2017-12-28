using DevPodcasts.DataLayer.Models;
using MediatR;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Library
{
    public class SubscribeUnsubscribeEmailNotification
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
                var userExists = _context.Users.Any(u => u.Id == message.UserId);
                if (!userExists)
                {
                    model.Error = "User doesn't exist";
                    return model;
                }

                var podcastExists = _context.Podcasts.Any(p => p.Id == message.PodcastId);
                if (!podcastExists)
                {
                    model.Error = "Podcast doesn't exist";
                    return model;
                }

                var libraryPodcast = await _context.LibraryPodcasts
                    .Where(p => p.PodcastId == message.PodcastId && p.UserId == message.UserId)
                    .SingleOrDefaultAsync();

                if (libraryPodcast == null)
                {
                    model.Error = "Library podcast doesn't exist for user";
                    return model;
                }

                libraryPodcast.IsSubscribed = !libraryPodcast.IsSubscribed;
                await _context.SaveChangesAsync();
                model.IsSuccess = true;

                return model;
            }
        }
    }
}