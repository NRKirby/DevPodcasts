﻿using Autofac;
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
        }

        private void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Logging
            builder.RegisterType<AzureTableLogger>().As<ILogger>();

            // Repositories
            builder.RegisterType<TagsRepository>();
            builder.RegisterType<EpisodeRepository>();
            builder.RegisterType<PodcastRepository>();

            // Services
            builder.RegisterType<AdminService>();
            builder.RegisterType<EpisodeService>();
            builder.RegisterType<EpisodeUpdater.EpisodeUpdater>().PropertiesAutowired();
            builder.RegisterType<HomeService>();
            builder.RegisterType<PodcastEmailService>();
            builder.RegisterType<PodcastService>();
            builder.RegisterType<RssParser>();
            builder.RegisterType<RssService>();
            builder.RegisterType<SearchService>();
            builder.RegisterType<LogService>().As<LogService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void UpdatePodcastEpisodes()
        {
            var updateEpisodes = System
                .Configuration
                .ConfigurationManager
                .AppSettings["UpdateOnStartup"]
                .ToLower();

            if (!updateEpisodes.Equals("true")) return;

            var updater = DependencyResolver.Current.GetService<EpisodeUpdater.EpisodeUpdater>();
            updater.Update();
        }
    }
}
