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

        public ActionResult Index(int? page, string filter)
        {
            const int itemsPerPage = 10;
            var viewModel = _logService.GetLogs(page ?? 0, itemsPerPage, filter);
            viewModel.ErrorLevels = new SelectList(new[]
            {
                new SelectListItem {Text = "Debug", Value = "Debug"},
                new SelectListItem {Text = "Error", Value = "Error"},
                new SelectListItem {Text = "Information", Value = "Information"},
                new SelectListItem {Text = "Warning", Value = "Warning"}
            }, "Text", "Value");

            return View(viewModel);
        }
    }
}