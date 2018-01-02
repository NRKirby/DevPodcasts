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
        [FunctionName("NotifyPodcastSubscribers")] // http://localhost:7071/api/NotifyPodcastSubscribers?key=theKey&episodeId=22&podcastId=141
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            PostData data = await req.Content.ReadAsAsync<PostData>();

            //const string connectionString = "Server=tcp:devpodcasts.database.windows.net,1433;Initial Catalog=devpodcasts;Persist Security Info=False;User ID=whiffwhaff9238;Password=mtisaIr2ESthVS64Kx7z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DevPodcasts02012018;Integrated Security=True";
            var context = new ApplicationDbContext(connectionString);


            var key = data?.Key;
            int episodeId = data.EpisodeId;
            var podcastId = context.Episodes.Where(episode => episode.Id == episodeId).Select(episode => episode.Podcast.Id).SingleOrDefault();

            if (key != "theKey")
                return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid key");

            var users = context.LibraryPodcasts.Where(p => p.PodcastId == podcastId).Select(p => p.ApplicationUser).ToList();

            // notify each user of new episode

            return req.CreateResponse(HttpStatusCode.OK, "Hello ");
        }
    }

    public class PostData
    {
        public string Key { get; set; }
        public int EpisodeId { get; set; }
    }
}
