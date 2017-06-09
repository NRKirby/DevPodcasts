using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ViewModels.Podcast;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastsController : Controller
    {
        private readonly PodcastService _podcastService;

        public PodcastsController(PodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public ActionResult Index()
        {
            var viewModel = _podcastService.Search();
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Submit()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Submit(SubmitPodcastViewModel model)
        {
            var result = await _podcastService.SubmitPodcastForReview(model);
            return View(result);
        }

        public ActionResult Detail(int id)
        {
            var podcastExists = _podcastService.PodcastExists(id);
            if (!podcastExists)
                return RedirectToAction("Index", "Home"); // TODO: redirect to error page

            var viewModel = _podcastService.GetPodcastDetail(id);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var viewModel = _podcastService.Edit(id);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(EditPodcastViewModel model)
        {
            await _podcastService.UpdatePodcast(model);
            return RedirectToAction("ManagePodcasts", "Admin");
        }
    }
}