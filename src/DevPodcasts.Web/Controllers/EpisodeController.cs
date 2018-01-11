using DevPodcasts.ServiceLayer.Episode;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly EpisodeService _episodeService;

        public EpisodeController(EpisodeService episodeService)
        {
            _episodeService = episodeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int? id)
        {
            if (id == null ||  !_episodeService.EpisodeExists((int)id))
                return RedirectToAction("Index", "Home"); // TODO: redirect to error page

            var viewModel = _episodeService.GetEpisodeDetail((int)id);
            return View(viewModel);
        }
    }
}