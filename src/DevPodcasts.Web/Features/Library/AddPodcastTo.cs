using System.Threading.Tasks;
using MediatR;

namespace DevPodcasts.Web.Features.Library
{
    public class AddPodcastTo
    {
        public class Command : IRequest
        {

        }

        public class CommandHandler : IAsyncRequestHandler<Command>
        {
            public Task Handle(Command message)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}