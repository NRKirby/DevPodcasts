using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.ServiceLayer;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastController : Controller
    {
        private readonly PodcastService _service;

        public PodcastController()
        {
            _service = new PodcastService();
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
            var result = await _service.AddPodcastForReview(model);
            return View();
        }
    }
}