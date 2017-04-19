using DevPodcasts.ViewModels.Home;

namespace DevPodcasts.ServiceLayer
{
    public interface IHomeService
    {
        HomeIndexViewModel GetIndexViewModel();
    }
}
