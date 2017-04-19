using DevPodcasts.ServiceLayer;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _episodeService;

        public EpisodeController(IEpisodeService episodeService)
        {
            _episodeService = episodeService;
        }

        public ActionResult Detail(int id)
        {
            var viewModel = _episodeService.GetEpisodeDetail(id);
            return View(viewModel);
        }
    }
}