using DevPodcasts.Models;
using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ServiceLayer.Tag;
using DevPodcasts.ViewModels.Podcast;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastsController : Controller
    {
        private readonly PodcastService _podcastService;
        private readonly TagService _tagService;

        public PodcastsController(PodcastService podcastService, TagService tagService)
        {
            _tagService = tagService;
            _podcastService = podcastService;
        }

        public ActionResult Index(int? page)
        {
            var viewModel = _podcastService.Search();
            return View(viewModel);
        }

        public ActionResult Tagged(string tagSlug)
        {
            if (!string.IsNullOrEmpty(tagSlug))
            {
                var viewModel = _tagService.GetTaggedPodcasts(tagSlug);
                return View(viewModel);
            }
            return View();
        }

        [Authorize]
        public ActionResult Submit()
        {
            var viewModel = new SubmitPodcastViewModel{ SuccessResult = SuccessResult.NotSet };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Submit(SubmitPodcastViewModel model)
        {
            var result = await _podcastService.SubmitPodcastForReview(model);
            return View(result);
        }

        public ActionResult Detail(int id)
        {
            var podcastExists = _podcastService.PodcastExists(id);
            if (!podcastExists)
                return RedirectToAction("Index", "Home"); // TODO: redirect to error page

            var viewModel = _podcastService.GetPodcastDetail(id);
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var viewModel = _podcastService.Edit(id);
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(EditPodcastViewModel model)
        {
            await _podcastService.UpdatePodcast(model);
            return RedirectToAction("ManagePodcasts", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int Id)
        {
            await _podcastService.DeletePodcast(Id);

            return RedirectToAction("ManagePodcasts", "Admin");
        }
    }
}