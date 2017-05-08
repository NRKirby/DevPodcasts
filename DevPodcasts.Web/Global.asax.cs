using Autofac;
using Autofac.Integration.Mvc;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Admin;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ServiceLayer.Episode;
using DevPodcasts.ServiceLayer.Home;
using DevPodcasts.ServiceLayer.Logging;
using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ServiceLayer.RSS;
using DevPodcasts.ServiceLayer.Search;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DevPodcasts.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            RegisterComponents();
            UpdatePodcastEpisodes();
            UpdatePodcastEpisodes();
        }

        private void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Logging
            builder.RegisterType<AzureTableLogger>().As<ILogger>(); builder.RegisterType<AzureTableLogger>().As<ILogger>();

            // Repositories
            builder.RegisterType<TagsRepository>().As<ITagsRepository>();
            builder.RegisterType<EpisodeRepository>().As<IEpisodeRepository>();
            builder.RegisterType<PodcastRepository>().As<IPodcastRepository>();

            // Services
            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<EpisodeService>().As<IEpisodeService>();
            builder.RegisterType<EpisodeUpdater>().PropertiesAutowired();
            builder.RegisterType<HomeService>().As<IHomeService>();
            builder.RegisterType<PodcastEmailService>().As<IPodcastEmailService>();
            builder.RegisterType<PodcastService>().As<IPodcastService>();
            builder.RegisterType<RssParser>().As<IRssParser>();
            builder.RegisterType<RssService>().As<IRssService>();
            builder.RegisterType<SearchService>().As<ISearchService>();
            builder.RegisterType<LogService>().As<LogService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void UpdatePodcastEpisodes()
        {
            var updater = DependencyResolver.Current.GetService<EpisodeUpdater>();
            updater.Update();
        }
    }
}
