using DevPodcasts.ServiceLayer.Logging;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LogsController : Controller
    {
        private readonly LogService _logService;
        private const int ItemsPerPage = 50;

        public LogsController(LogService logService)
        {
            _logService = logService;
        }

        public ActionResult Index(int? page, string filter)
        {
            if (filter == "Select a level")
                filter = null;

            var viewModel = _logService.GetIndexViewModel(page ?? 0, ItemsPerPage, filter);
            ViewBag.Filter = filter;
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Query(int? page, string filter)
        {
            if (filter == "Select a level")
                filter = null;

            var logs = _logService.GetLogs(page ?? 0, ItemsPerPage, filter);
            ViewBag.Filter = filter;

            return Json(new { filter = filter, html = PartialView("_LogResultsTable", logs) });
        }
    }
}