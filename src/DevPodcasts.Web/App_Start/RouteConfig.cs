using System.Web.Mvc;
using System.Web.Routing;
using DevPodcasts.Web.Controllers;

namespace DevPodcasts.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                "TagsIndex",
                "Tags/Index",
                new { controller = "Tags", action = "Index" }
            );

            routes.MapRoute(
                "Tags",
                "Tags/{tagSlug}",
                new { controller = "Tags", action = "Index" }
            );

            routes.MapRoute(
                "Podcasts",
                "Podcasts/Tagged/{tagSlug}",
                new { controller = "Podcasts", action = "Tagged", tagSlug = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
