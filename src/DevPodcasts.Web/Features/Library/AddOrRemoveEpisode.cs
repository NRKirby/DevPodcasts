using DevPodcasts.DataLayer.Models;
using MediatR;
using System;
using System.Data.Entity;
using System.Linq;
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
                var model = new AddRemoveLibraryAjaxModel { IsSuccess = false };
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == message.UserId);
                if (user == null)
                {
                    model.Error = "User doesn't exist";
                    return model;
                }

                var episode = await _context.Episodes.SingleOrDefaultAsync(p => p.Id == message.EpisodeId);
                if (episode == null)
                {
                    model.Error = "Episode doesn't exist";
                    return model;
                }

                var userIsSubscribed = user.LibraryEpisodes.Any(p => p.EpisodeId == episode.Id);
                if (userIsSubscribed)
                {
                    var libraryEpisode = await _context.LibraryEpisodes.SingleAsync(e => e.EpisodeId == episode.Id);
                    _context.LibraryEpisodes.Remove(libraryEpisode);
                    user.LibraryEpisodes.Remove(libraryEpisode);
                    model.IsAdded = false;
                }
                else
                {
                    var libraryEpisode = new LibraryEpisode
                    {
                        EpisodeId = episode.Id,
                        Episode = episode,
                        UserId = user.Id,
                        ApplicationUser = user,
                        DateAdded = DateTime.Now
                    };
                    user.LibraryEpisodes.Add(libraryEpisode);
                    _context.LibraryEpisodes.Add(libraryEpisode);
                    model.IsAdded = true;
                }

                model.IsSuccess = true;
                await _context.SaveChangesAsync();

                return model;
            }
        }
    }
}