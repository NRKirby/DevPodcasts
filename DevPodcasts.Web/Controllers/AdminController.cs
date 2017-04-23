using DevPodcasts.ServiceLayer;
using DevPodcasts.ViewModels.Admin;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public ActionResult Index()
        {
            var viewModel = _adminService.GetIndexViewModel();
            return View(viewModel);
        }

        public ActionResult ReviewSubmission(int podcastId)
        {
            var viewModel = _adminService.GetPodcastForReview(podcastId);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ReviewSubmission(ReviewPodcastViewModel model)
        {
            var selectedCategories = model.Tags.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            _adminService.Save(model.Id, selectedCategories);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Reject(int podcastId)
        {
            await _adminService.Reject(podcastId);
            return RedirectToAction("Index");
        }

        public ActionResult ManagePodcasts()
        {
            var viewModel = _adminService.GetPodcastList();
            return View(viewModel);
        }
    }
}