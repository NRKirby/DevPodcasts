using DevPodcasts.Models;
using DevPodcasts.ServiceLayer.RSS;
using System;
using System.Configuration;
using System.Web.Http;

namespace DevPodcasts.Web.Controllers.Api
{
    public class UpdateEpisodesController : ApiController
    {
        private RssService _rssService;

        public UpdateEpisodesController(RssService rssService)
        {
            _rssService = rssService;
        }

        public IHttpActionResult Post([FromBody]UpdateEpisodesModel model)
        {
            var key = ConfigurationManager.AppSettings["WebApiKey"];

            if (model.Key != key)
                return Unauthorized();

            switch (model.UpdateType)
            {
                case UpdateType.Sync:
                    _rssService.UpdateEpisodes();
                    break;
                case UpdateType.Async:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException("Specify update type (i.e. Sync");
            }

            return Ok();
        }
    }
}

