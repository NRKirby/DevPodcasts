using DevPodcasts.ServiceLayer.Episode;
using DevPodcasts.Web.Features.Episode;
using DevPodcasts.Web.Features.Library;
using MediatR;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly EpisodeService _episodeService;
        private readonly IMediator _mediator;

        public EpisodeController(IMediator mediator, EpisodeService episodeService)
        {
            _episodeService = episodeService;
            _mediator = mediator;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null ||  !_episodeService.EpisodeExists((int)id))
                return RedirectToAction("Index", "Home"); // TODO: redirect to error page

            var userId = User.Identity.GetUserId();
            var viewModel = await _mediator.Send(new Detail.Query {EpisodeId = (int)id, UserId = userId });

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddRemove(AddRemoveLibraryAjaxModel model)
        {
            var viewModel = await _mediator.Send(new AddOrRemoveEpisode.Command { UserId = model.U, EpisodeId = model.P });

            return Json(viewModel);
        }
    }
}