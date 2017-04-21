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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var episodeExists = _episodeService.EpisodeExists(id);
            if (!episodeExists)
                return RedirectToAction("Index", "Home"); // TODO: redirect to error page

            var viewModel = _episodeService.GetEpisodeDetail(id);
            return View(viewModel);
        }
    }
}