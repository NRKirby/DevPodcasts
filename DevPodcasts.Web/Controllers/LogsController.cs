using DevPodcasts.ServiceLayer.Logging;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogsController : Controller
    {
        private readonly LogService _logService;

        public LogsController(LogService logService)
        {
            _logService = logService;
        }

        public ActionResult Index(int? page)
        {
            const int itemsPerPage = 100;
            var viewModel = _logService.GetLogs(page ?? 0, itemsPerPage);

            return View(viewModel);
        }
    }
}