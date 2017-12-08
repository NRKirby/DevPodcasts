using System.Threading.Tasks;
using System.Web.Mvc;
using DevPodcasts.Web.Features.Library.Index;
using MediatR;
using Microsoft.AspNet.Identity;

namespace DevPodcasts.Web.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IMediator _mediator;

        public LibraryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Podcasts()
        {
            if (!User.Identity.IsAuthenticated) return View();

            var viewModel = await _mediator.Send(new List.Query { UserId = User.Identity.GetUserId() });
            return View(viewModel);
        }

        public async Task<ActionResult> Episodes()
        {
            if (!User.Identity.IsAuthenticated) return View();

            var viewModel = await _mediator.Send(new List.Query { UserId = User.Identity.GetUserId() });
            return View(viewModel);
        }
    }
}