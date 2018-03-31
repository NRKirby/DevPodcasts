using DevPodcasts.DataLayer.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevPodcasts.NotifyPodcastSubscribers
{
    public static class Function
    {
        [FunctionName("NotifyPodcastSubscribers")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var data = await req.Content.ReadAsAsync<PostData>();

            if (data?.AccessKey != ConfigurationManager.AppSettings["AccessKey"])
                return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid key");

            var connectionString = ConfigurationManager.ConnectionStrings["SqlDb"].ConnectionString;
            var context = new ApplicationDbContext(connectionString);

            var episodeId = data.EpisodeId;
            var episode = context.Episodes.SingleOrDefault(e => e.Id == episodeId);
            var podcast = episode.Podcast;

            log.Info($"{podcast.Title}: {episode.Title}");
            log.Info($"*********************");

            // notify each user of new episode
            var users = context.LibraryPodcasts.Where(p => p.PodcastId == podcast.Id).Select(p => p.ApplicationUser).ToList();
            var count = 0;
            foreach (var user in users)
            {
                await SendEmail(user, episode);
                log.Info($"Email sent to {user.Email}");
                count++;
            }

            log.Info($"{count} emails sent");

            return req.CreateResponse(HttpStatusCode.OK, $"{count} emails sent");
        }

        private static async Task SendEmail(ApplicationUser user, Episode episode)
        {
            var podcast = episode.Podcast;

            var apiKey = ConfigurationManager.AppSettings["SendGridKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("notifications@devpodcasts.net", "Dev Podcasts");
            var subject = $"New {podcast.Title} episode available!";
            var to = new EmailAddress($"{user.Email}", $"{user.FirstName} {user.LastName}");

            var plainTextContent = $"New episode: {episode.Title} available\ndevpodcasts.net/episodes/detail/{episode.Id}";
            var htmlContent = $@"<h1>Dev Podcasts</h1>
                                 <p>New episode <strong>{episode.Title}</strong> available</p>
                                 <a href=""https://devpodcasts.net/episode/detail/{episode.Id}"">https://devpodcasts.net/episode/detail/{episode.Id}</a>
                                 <p><strong>Summary:</strong></p>
                                 <p>{episode.Summary}</p>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
        }
    }
}
