using DevPodcasts.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace DevPodcasts.Web.Controllers
{
    public class UpdateEpisodesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public IHttpActionResult Post([FromBody]UpdateEpisodesModel model)
        {
            var key = ConfigurationManager.AppSettings["WebApiKey"];

            if (model.Key != key)
                return Unauthorized();

            return Ok();
        }
    }
}

