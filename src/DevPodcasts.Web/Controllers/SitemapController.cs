using DevPodcasts.Web.Features.Sitemap;
using MediatR;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevPodcasts.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IMediator _mediator;

        public SitemapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [OutputCache(Duration = 30, VaryByParam = "none")]
        public async Task<ActionResult> Index()
        {
            var sitemapItems = await _mediator.Send(new GenerateSitemapItems.Query());

            return new SitemapResult(sitemapItems);
        }
    }
}