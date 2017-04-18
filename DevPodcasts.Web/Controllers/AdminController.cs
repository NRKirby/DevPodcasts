using DevPodcasts.ServiceLayer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var viewModel = _service.GetIndexViewModel();
            return View(viewModel);
        }

        public ActionResult Approve(int podcastId)
        {
            _service.Approve(podcastId);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Reject(int podcastId)
        {
            await _service.Reject(podcastId);
            return RedirectToAction("Index");
        }

        public ActionResult ReviewSubmission(int podcastId)
        {
            var viewModel = _service.GetPodcastForReview(podcastId);
            return View(viewModel);
        }
    }
}