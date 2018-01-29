using Autofac;
using Autofac.Features.Variance;
using DevPodcasts.DataLayer.Models;
using DevPodcasts.Web;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevPodcasts.Tests
{
    public abstract class TestBase
    {
        protected IMediator Mediator;

        protected TestBase()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationDbContext>();

            Register.RegisterTypes(builder);
            var container = builder.Build();

            Mediator = container.Resolve<IMediator>(new NamedParameter("IMediator", this));
        }

        public static class Register
        {
            public static void RegisterTypes(ContainerBuilder builder)
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
        }
    }
}
