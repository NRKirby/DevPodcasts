using DevPodcasts.DataLayer.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevPodcasts.NotifyPodcastSubscribers
{
    public static class Function
    {
        [FunctionName("NotifyPodcastSubscribers")] // http://localhost:7071/api/NotifyPodcastSubscribers
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            PostData data = await req.Content.ReadAsAsync<PostData>();

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureSqlDb"].ConnectionString;
            var context = new ApplicationDbContext(connectionString);


            var key = data?.Key;
            int episodeId = data.EpisodeId;
            var podcastId = context.Episodes.Where(episode => episode.Id == episodeId).Select(episode => episode.Podcast.Id).SingleOrDefault();

            if (key != "theKey")
                return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid key");

            var users = context.LibraryPodcasts.Where(p => p.PodcastId == podcastId).Select(p => p.ApplicationUser).ToList();

            // notify each user of new episode
            foreach (var user in users)
            {

            }

            return req.CreateResponse(HttpStatusCode.OK, "Hello ");
        }
    }
}
