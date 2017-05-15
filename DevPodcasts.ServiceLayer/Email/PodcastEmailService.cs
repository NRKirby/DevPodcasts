using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Email
{
    public class PodcastEmailService
    {
        public async Task SendPodcastSubmittedEmailAsync(string podcastTitle)
        {
            var client = new SendGridClient(EmailConstants.ApiKey);
            var from = new EmailAddress(EmailConstants.AdminNoReplyAddress, EmailConstants.DevPodcasts);
            var msg = new SendGridMessage
            {
                From = from,
                Subject = $"{podcastTitle} submitted for review",
                HtmlContent = $"{podcastTitle} submitted for review"
            };
            var to = new EmailAddress(EmailConstants.AdminEmailAddress);
            msg.AddTo(to);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
