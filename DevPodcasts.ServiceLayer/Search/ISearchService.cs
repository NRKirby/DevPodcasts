using DevPodcasts.ViewModels.Search;

namespace DevPodcasts.ServiceLayer.Search
{
    public interface ISearchService
    {
        SearchIndexViewModel Search(string query, string type = "podcast");
    }
}
