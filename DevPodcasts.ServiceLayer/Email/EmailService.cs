using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Email
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await SendWithSendGridAsync(message);
        }

        private async Task SendWithSendGridAsync(IdentityMessage message)
        {
            var apiKey = "";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@devpodcasts.net", "Dev Podcasts");
            var bcc = new EmailAddress("nrkirb@gmail.com");
            var msg = new SendGridMessage()
            {
                From = from,
                Subject = message.Subject,
                HtmlContent = message.Body
            };
            msg.AddTo(new EmailAddress(message.Destination));
            //msg.AddBcc(bcc);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
