using System.Web.Mvc;
using DevPodcasts.ServiceLayer;

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

            return View();
        }
    }
}