using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Email
{
    public interface IPodcastEmailService
    {
        Task SendPodcastSubmittedEmailAsync(string podcastTitle);
    }
}