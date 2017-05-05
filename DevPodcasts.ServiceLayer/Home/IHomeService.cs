using DevPodcasts.ViewModels.Home;

namespace DevPodcasts.ServiceLayer.Home
{
    public interface IHomeService
    {
        HomeIndexViewModel GetIndexViewModel();
    }
}
