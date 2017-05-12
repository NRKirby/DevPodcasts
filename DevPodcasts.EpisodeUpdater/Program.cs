using System.Web.Mvc;

namespace DevPodcasts.EpisodeUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var updater = DependencyResolver.Current.GetService<EpisodeUpdater>();
            updater.Update();
        }
    }
}
