using DevPodcasts.ViewModels.Search;

namespace DevPodcasts.ServiceLayer
{
    public interface ISearchService
    {
        SearchIndexViewModel Search(string query, string type = "podcast");
    }
}
