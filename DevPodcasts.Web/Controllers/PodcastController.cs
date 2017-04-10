using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.ServiceLayer;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastController : Controller
    {
        private PodcastService _service;

        public PodcastController()
        {
            _service = new PodcastService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddPodcastViewModel model)
        {
            var result = await _service.Add(model);
            return View();
        }
    }
}