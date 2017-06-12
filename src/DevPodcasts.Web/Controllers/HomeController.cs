using DevPodcasts.ServiceLayer;
using System.Web.Mvc;
using DevPodcasts.ServiceLayer.Home;
using DevPodcasts.ViewModels.Home;

namespace DevPodcasts.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public ActionResult Index()
        {
            var viewModel = _homeService.GetIndexViewModel();
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactViewModel model)
        {

            return View();
        }
    }
}