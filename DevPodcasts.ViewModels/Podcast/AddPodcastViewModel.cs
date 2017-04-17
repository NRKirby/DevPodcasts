using DevPodcasts.Dtos;

namespace DevPodcasts.ViewModels.Podcast
{
    public class SubmitPodcastViewModel
    {
        public string RssFeedUrl { get; set; }

        public SuccessResult SuccessResult { get; set; }
    }
}
