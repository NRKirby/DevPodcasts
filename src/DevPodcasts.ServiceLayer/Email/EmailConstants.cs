using System.Configuration;

namespace DevPodcasts.ServiceLayer.Email
{
    public static class EmailConstants
    {
        public static string ApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
        public const string AdminEmailAddress = "nrkirb@gmail.com";
        public const string AdminNoReplyAddress = "no-reply@devpodcasts.net";
        public const string DevPodcasts = "Dev Podcasts";
        public const string GCaptchaSecret = "6LeAoA8UAAAAAHLLZXNxdUxnWsj5wNCtPPvecs8o";

        public const string FeedbackResponseMessage = "Thanks for your feedback!";
        public const string ReportAnIssueResponseMessage = "Thanks for letting us know about your issue, we will get back to you as soon as we can.";
        public const string ReportABugResponseMessage = "Thanks for letting us know about the bug, we will try to resolve it as soon as possible and will contact you if we need any more information.";
        public const string SuggestAFeatureResponseMessage = "Thanks for your suggestion, we are always trying to improve the site and appreciate you taking the time to help us.";
    }
}
