using DevPodcasts.ServiceLayer.Search;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }

        public ActionResult Index(string q)
        {
            if (q.Equals(string.Empty))
                return View();

            ViewBag.SearchQuery = q;
            var viewModel = _searchService.Search(q);
            return View(viewModel);
        }
    }
}