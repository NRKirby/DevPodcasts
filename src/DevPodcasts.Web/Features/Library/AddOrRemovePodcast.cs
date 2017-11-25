using System.Threading.Tasks;
using MediatR;
using DevPodcasts.DataLayer.Models;
using System.Data.Entity;

namespace DevPodcasts.Web.Features.Library
{
    public class AddOrRemovePodcast
    {
        public class Command : IRequest<AddRemoveModel>
        {
            public string UserId { get; set; }
            public int PodcastId { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, AddRemoveModel>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<AddRemoveModel> Handle(Command message)
            {
                var model = new AddRemoveModel { IsSuccess = false };
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

                var userIsSubscribed = user.SubscribedPodcasts.Contains(podcast);
                if (userIsSubscribed)
                {
                    user.SubscribedPodcasts.Remove(podcast);
                }
                else
                {
                    user.SubscribedPodcasts.Add(podcast);
                }

                model.IsSuccess = true;
                await _context.SaveChangesAsync();

                return model;
            }
        }
    }
}