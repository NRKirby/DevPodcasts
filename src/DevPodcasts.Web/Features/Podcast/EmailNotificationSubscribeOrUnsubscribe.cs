using System.Threading.Tasks;
using DevPodcasts.DataLayer.Models;
using DevPodcasts.Web.Features.Library;
using MediatR;

namespace DevPodcasts.Web.Features.Podcast
{
    public class EmailNotificationSubscribeOrUnsubscribe
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

            public Task<AjaxModel> Handle(Command message)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}