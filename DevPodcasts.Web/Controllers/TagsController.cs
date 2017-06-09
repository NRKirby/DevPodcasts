using System.Web.Mvc;
using DevPodcasts.ServiceLayer.Tag;

namespace DevPodcasts.Web.Controllers
{
    public class TagsController : Controller
    {
        private readonly TagService _tagService;

        public TagsController(TagService tagService)
        {
            _tagService = tagService;
        }

        public ActionResult Index(string tagSlug)
        {
            var viewModel = _tagService.GetTags();

            return View(viewModel);
        }
    }
}