using Autofac;
using Autofac.Features.Variance;
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
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            var createRoles = bool.Parse(ConfigurationManager.AppSettings["CreateRoles"]);

            if (createRoles)
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

        private static void RegisterComponents()
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

            SetUpMediator(builder);
            SetUpMicrosoftIdentity(builder);

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void SetUpMediator(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).Where(t =>
                    t.GetInterfaces().Any(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>))
                                               || i.IsClosedTypeOf(typeof(IRequestHandler<>))
                                               || i.IsClosedTypeOf(typeof(IAsyncRequestHandler<,>))
                                               || i.IsClosedTypeOf(typeof(IAsyncRequestHandler<>))
                                               || i.IsClosedTypeOf(typeof(ICancellableAsyncRequestHandler<,>))
                                               || i.IsClosedTypeOf(typeof(INotificationHandler<>))
                                               || i.IsClosedTypeOf(typeof(IAsyncNotificationHandler<>))
                                               || i.IsClosedTypeOf(typeof(ICancellableAsyncNotificationHandler<>))
                    )
                )
                .AsImplementedInterfaces();

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t =>
                {
                    object o;
                    return c.TryResolve(t, out o) ? o : null;
                };
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
        }

        private static void SetUpMicrosoftIdentity(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationDbContext>().AsSelf().InstancePerRequest();
            builder.Register(c => new UserStore<ApplicationUser>(c.Resolve<ApplicationDbContext>())).AsImplementedInterfaces().InstancePerRequest();

            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<Microsoft.Owin.Security.IAuthenticationManager>();

            builder.Register<IDataProtectionProvider>(c => Startup.DataProtectionProvider).InstancePerRequest();
        }
    }
}