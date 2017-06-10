using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevPodcasts.Web.Startup))]
namespace DevPodcasts.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
