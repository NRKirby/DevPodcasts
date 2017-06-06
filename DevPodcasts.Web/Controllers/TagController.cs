using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class TagController : Controller
    { 
        public ActionResult Index(string tagValue = null)
        {
            return View();
        }
    }
}