using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.ServiceLayer;
using System.Web.Mvc;
using System.Threading.Tasks;

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
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Add(AddPodcastViewModel model)
        {
            var result = await _podcastService.AddPodcastForReview(model);
            return View();
        }
    }
}