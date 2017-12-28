using System.Threading.Tasks;
using MediatR;

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
            public Task<AjaxModel> Handle(Command message)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}