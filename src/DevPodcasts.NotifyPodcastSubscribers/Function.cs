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
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string key = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "key", true) == 0)
                .Value;

            if (key != "theKey")
                return req.CreateResponse(HttpStatusCode.BadRequest, "Invalid key");

            int episodeId = int.Parse(req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "episodeId", true) == 0)
                .Value);

            int podcastId = int.Parse(req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "podcastId", true) == 0)
                .Value);

            //const string connectionString = "Server=tcp:devpodcasts.database.windows.net,1433;Initial Catalog=devpodcasts;Persist Security Info=False;User ID=whiffwhaff9238;Password=mtisaIr2ESthVS64Kx7z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=DevPodcasts02012018;Integrated Security=True";
            var context = new ApplicationDbContext(connectionString);

            var users = context.LibraryPodcasts.Where(p => p.PodcastId == podcastId).Select(p => p.ApplicationUser).ToList();

            // notify each user of new episode

            return req.CreateResponse(HttpStatusCode.OK, "Hello " + key);
        }
    }
}
