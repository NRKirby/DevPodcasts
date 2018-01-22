using DevPodcasts.Web.Features.Library;
using MediatR;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IMediator _mediator;

        public LibraryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Episodes");
        }

        public async Task<ActionResult> Podcasts()
        {
            if (!User.Identity.IsAuthenticated) return View();

            var viewModel = await _mediator.Send(new ListPodcasts.Query { UserId = User.Identity.GetUserId() });
            return View(viewModel);
        }

        public async Task<ActionResult> Episodes()
        {
            if (!User.Identity.IsAuthenticated) return View();

            var viewModel = await _mediator.Send(new ListEpisodes.Query { UserId = User.Identity.GetUserId() });
            return View(viewModel);
        }
    }
}