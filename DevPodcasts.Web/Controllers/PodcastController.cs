using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.ServiceLayer;
using System.Web.Mvc;

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
        public ActionResult Add(AddPodcastViewModel model)
        {
            var result = _service.Add(model);
            return View();
        }
    }
}