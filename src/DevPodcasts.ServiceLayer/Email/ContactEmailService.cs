using DevPodcasts.ViewModels.Home;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Email
{
    public class ContactEmailService
    {
        public async Task<string> SendAsync(ContactViewModel model)
        {
            var captchaResponse = await ValidateCaptchaResponse(model.GCaptchaResponse);

            string captchaMessage;
            if (captchaResponse.Success)
            {
                await SendEmailAsync(model);
                captchaMessage = "Valid";
            }
            else
            {
                captchaMessage = "An error occured. Please try again";
            }

            return captchaMessage;
        }

        private async Task<CaptchaResponse> ValidateCaptchaResponse(string gCaptchaResponse)
        {
            var webClient = new WebClient();
            var reply = await webClient.DownloadStringTaskAsync($"https://www.google.com/recaptcha/api/siteverify?secret={EmailConstants.GCaptchaSecret}&response={gCaptchaResponse}");

            return JsonConvert.DeserializeObject<CaptchaResponse>(reply);
        }

        private static async Task SendEmailAsync(ContactViewModel model)
        {
            var client = new SendGridClient(EmailConstants.ApiKey);
            var from = new EmailAddress(model.EmailAddress, EmailConstants.DevPodcasts);
            var msg = new SendGridMessage
            {
                From = from,
                Subject = model.Subject,
                HtmlContent = model.Message
            };
            msg.AddTo(new EmailAddress(EmailConstants.AdminEmailAddress));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
