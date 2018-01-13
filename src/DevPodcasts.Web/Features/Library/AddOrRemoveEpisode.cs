using DevPodcasts.DataLayer.Models;
using MediatR;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Library
{
    public class AddOrRemoveEpisode
    {
        public class Command : IRequest<AddRemoveLibraryAjaxModel>
        {
            public string UserId { get; set; }
            public int EpisodeId { get; set; }
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
                return null;
            }
        }
    }
}