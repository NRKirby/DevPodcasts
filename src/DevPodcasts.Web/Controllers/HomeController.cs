using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ServiceLayer.Home;
using DevPodcasts.ViewModels.Home;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;
        private readonly ContactEmailService _emailService;

        public HomeController(HomeService homeService, ContactEmailService emailService)
        {
            _homeService = homeService;
            _emailService = emailService;
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContactViewModel model)
        {
            model.GCaptchaResponse = Request["g-recaptcha-response"];
            await _emailService.SendAsync(model);

            return View();
        }
    }
}