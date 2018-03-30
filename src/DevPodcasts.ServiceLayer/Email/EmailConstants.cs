using System.Configuration;

namespace DevPodcasts.ServiceLayer.Email
{
    public static class EmailConstants
    {
        public static string ApiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
        public const string AdminEmailAddress = "nrkirb@gmail.com";
        public const string AdminNoReplyAddress = "no-reply@devpodcasts.net";
        public const string DevPodcasts = "Dev Podcasts";

        public const string ContactResponseMessage = "Thanks for contacting us, we will contact you as soon as we can.";
    }
}
