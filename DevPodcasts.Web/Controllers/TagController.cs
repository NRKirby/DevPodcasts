using System.Web.Mvc;
using DevPodcasts.ServiceLayer.Tag;

namespace DevPodcasts.Web.Controllers
{
    public class TagController : Controller
    {
        private readonly TagService _tagService;

        public TagController(TagService tagService)
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