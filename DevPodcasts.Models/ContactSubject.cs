using System.ComponentModel.DataAnnotations;

namespace DevPodcasts.Models
{
    public enum ContactSubject
    {
        [Display(Name = "Feedback")]
        Feedback = 0,

        [Display(Name = "Report an issue")]
        ReportAnIssue = 1,

        [Display(Name = "Report a bug")]
        ReportABug = 2,

        [Display(Name = "Suggest a feature")]
        SuggestAFeature = 3
    }
}
