using DevPodcasts.ServiceLayer;
using DevPodcasts.ViewModels.Admin;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _service;

        public AdminController()
        {
            _service = new AdminService();
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

        public ActionResult Reject(int podcastId)
        {
            _service.Reject(podcastId);
            return RedirectToAction("Index");
        }
    }
}