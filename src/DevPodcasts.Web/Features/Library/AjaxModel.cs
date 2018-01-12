namespace DevPodcasts.Web.Features.Library
{
    public class AddRemovePodcastAjaxModel
    {
        public string U { get; set; }
        public int P { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsAdded { get; set; }
        public string Error { get; set; }
    }
}