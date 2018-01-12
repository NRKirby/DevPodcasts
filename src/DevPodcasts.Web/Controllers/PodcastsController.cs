using DevPodcasts.Models;
using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ServiceLayer.Tag;
using DevPodcasts.ViewModels.Podcast;
using DevPodcasts.Web.Features.Library;
using DevPodcasts.Web.Features.Podcast;
using MediatR;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastsController : Controller
    {
        private readonly PodcastService _podcastService;
        private readonly TagService _tagService;
        private readonly IMediator _mediator;

        public PodcastsController(
            PodcastService podcastService, 
            TagService tagService,
            IMediator mediator)
        {
            _tagService = tagService;
            _podcastService = podcastService;
            _mediator = mediator;
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

        public async Task<ActionResult> Detail(int? id)
        {
            if (id == null || !_podcastService.PodcastExists((int)id))
                return RedirectToAction("Index", "Home"); // TODO: redirect to error page

            var userId = User.Identity.GetUserId();
            var viewModel = await _mediator.Send(new Detail.Query { PodcastId = (int)id, UserId = userId });

            return View(viewModel);
        }

        private bool IsInteger(object value)
        {
            return false;
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

        [HttpPost]
        public async Task<ActionResult> AddRemove(AddRemovePodcastAjaxModel model)
        {
            var viewModel = await _mediator.Send(new AddOrRemovePodcast.Command { UserId = model.U, PodcastId = model.P } );

            return Json(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SubscribeUnsubscribeEmail(AddRemovePodcastAjaxModel model)
        {
            var viewModel = await _mediator.Send(new SubscribeUnsubscribeEmailNotification.Command { UserId = model.U, PodcastId = model.P });

            return Json(viewModel);
        }
    }
}