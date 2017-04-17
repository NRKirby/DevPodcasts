using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.ServiceLayer;
using System.Web.Mvc;
using System.Threading.Tasks;
using DevPodcasts.Dtos;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;

        public PodcastController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public ActionResult Index()
        {
            return View();
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
    }
}