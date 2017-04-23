using Autofac;
using Autofac.Integration.Mvc;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer;
using DevPodcasts.ServiceLayer.Email;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevPodcasts.ServiceLayer.Logging;

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
            RegisterComponents();
            UpdatePodcastEpisodes();
        }

        private void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Services
            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<EpisodeService>().As<IEpisodeService>();
            builder.RegisterType<HomeService>().As<IHomeService>();
            builder.RegisterType<PodcastService>().As<IPodcastService>();
            builder.RegisterType<PodcastEmailService>().As<IPodcastEmailService>();
            builder.RegisterType<RssService>().As<IRssService>();
            builder.RegisterType<RssParser>().As<IRssParser>();
            builder.RegisterType<FileLogger>().As<ILogger>();
            builder.RegisterType<SearchService>().As<ISearchService>();
            builder.RegisterType<EpisodeUpdater>().PropertiesAutowired();


            // Repositories
            builder.RegisterType<TagsRepository>().As<ITagsRepository>();
            builder.RegisterType<EpisodeRepository>().As<IEpisodeRepository>();
            builder.RegisterType<PodcastRepository>().As<IPodcastRepository>();
            
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
