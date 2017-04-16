using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Email
{
    public class AccountEmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var client = new SendGridClient(EmailConstants.ApiKey);
            var from = new EmailAddress(EmailConstants.AdminNoReplyAddress, EmailConstants.DevPodcasts);
            var bcc = new EmailAddress(EmailConstants.AdminEmailAddress);
            var msg = new SendGridMessage
            {
                From = from,
                Subject = message.Subject,
                HtmlContent = message.Body
            };
            msg.AddTo(new EmailAddress(message.Destination));
            msg.AddBcc(bcc);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
