using System.Data.Entity;
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

                var userIsSubscribed = user.SubscribedPodcasts.Contains(podcast);
                if (userIsSubscribed)
                {
                    user.SubscribedPodcasts.Remove(podcast);
                    model.IsAdded = false;
                }
                else
                {
                    user.SubscribedPodcasts.Add(podcast);
                    model.IsAdded = true;
                }

                model.IsSuccess = true;
                await _context.SaveChangesAsync();

                return model;
            }
        }
    }
}