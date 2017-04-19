﻿using Autofac;
using Autofac.Integration.Mvc;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer;
using DevPodcasts.ServiceLayer.Email;
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
            RegisterComponents();
            //UpdaterTest();
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

            // Repositories
            builder.RegisterType<CategoriesRepository>().As<ICategoriesRepository>();
            builder.RegisterType<EpisodeRepository>().As<IEpisodeRepository>();
            builder.RegisterType<PodcastRepository>().As<IPodcastRepository>();
            
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void UpdaterTest()
        {
            var updater = new EpisodeUpdater();
            updater.Update();
        }
    }
}
