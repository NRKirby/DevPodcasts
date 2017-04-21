using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult Index(string query)
        {
            return View();
        }
    }
}