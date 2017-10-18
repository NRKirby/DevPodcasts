using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DevPodcasts.DataLayer.Models;
using DevPodcasts.Logging;
using DevPodcasts.Repositories;
using DevPodcasts.ServiceLayer.Admin;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ServiceLayer.Episode;
using DevPodcasts.ServiceLayer.Home;
using DevPodcasts.ServiceLayer.Logging;
using DevPodcasts.ServiceLayer.Podcast;
using DevPodcasts.ServiceLayer.RSS;
using DevPodcasts.ServiceLayer.Search;
using DevPodcasts.ServiceLayer.Tag;
using DevPodcasts.Web.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Http;
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
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MvcHandler.DisableMvcResponseHeader = true;
            RegisterComponents();
            CreateRolesIfNotPresentInDatabase();
            WarmUpEntityFrameworkQueries();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void WarmUpEntityFrameworkQueries()
        {
            new SearchService(new PodcastRepository()).Search("test");
            new TagService().GetTaggedPodcasts("test");
        }

        private static void CreateRolesIfNotPresentInDatabase()
        {
            var createRoles = ConfigurationManager.AppSettings["CreateRoles"];

            if (createRoles.ToLower().Equals("true"))
            {
                var context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var roleNames = GetRoleNameList();

                foreach (var roleName in roleNames)
                {
                    if (!roleManager.RoleExists(roleName))
                    {
                        roleManager.Create(new IdentityRole(roleName));
                    }
                }
            }
        }

        private static IEnumerable<string> GetRoleNameList()
        {
            return new List<string>
            {
                "Admin"
            };
        }

        private void RegisterComponents()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Context 
            builder.RegisterType<ApplicationDbContext>();

            // Logging
            builder.RegisterType<AzureTableLogger>().As<ILogger>();

            // Repositories
            builder.RegisterType<TagsRepository>();
            builder.RegisterType<EpisodeRepository>();
            builder.RegisterType<PodcastRepository>();

            // Services
            builder.RegisterType<AdminService>();
            builder.RegisterType<ContactEmailService>();
            builder.RegisterType<EpisodeService>();
            builder.RegisterType<HomeService>();
            builder.RegisterType<PodcastEmailService>();
            builder.RegisterType<PodcastService>();
            builder.RegisterType<RssParser>();
            builder.RegisterType<RssService>();
            builder.RegisterType<SearchService>();
            builder.RegisterType<TagService>();
            builder.RegisterType<LogService>().As<LogService>();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}