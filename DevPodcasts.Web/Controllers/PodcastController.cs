﻿using DevPodcasts.ServiceLayer;
using DevPodcasts.ViewModels.Podcast;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;

        public PodcastController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public ActionResult Index()
        {
            var viewModel = _podcastService.Search();
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Submit()
        {
            return View();
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
        public ActionResult Edit(EditPodcastViewModel model)
        {
            _podcastService.UpdatePodcast(model);
            return RedirectToAction("ManagePodcasts", "Admin");
        }
    }
}