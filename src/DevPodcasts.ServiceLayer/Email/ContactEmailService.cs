using DevPodcasts.ViewModels.Home;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;

namespace DevPodcasts.ServiceLayer.Email
{
    public class ContactEmailService
    {
        public async Task<ContactViewModel> SendAsync(ContactViewModel model)
        {
            var captchaResponse = await ValidateCaptchaResponse(model.GCaptchaResponse);

            if (captchaResponse.Success)
            {
                await SendEmailAsync(model);
                model.IsSuccess = true;
                model.SuccessMessage = EmailConstants.ContactResponseMessage;
            }
            else
            {
                model.IsSuccess = false;
            }

            return model;
        }

        private static async Task<CaptchaResponse> ValidateCaptchaResponse(string gCaptchaResponse)
        {
            var gCaptchaSecret = ConfigurationManager.AppSettings["GCaptchaSecret"];
            var webClient = new WebClient();
            var reply = await webClient.DownloadStringTaskAsync($"https://www.google.com/recaptcha/api/siteverify?secret={gCaptchaSecret}&response={gCaptchaResponse}");

            return JsonConvert.DeserializeObject<CaptchaResponse>(reply);
        }

        private static async Task SendEmailAsync(ContactViewModel model)
        {
            var client = new SendGridClient(EmailConstants.ApiKey);
            var from = new EmailAddress(model.EmailAddress, EmailConstants.DevPodcasts);
            var subject = model.Subject;
            var msg = new SendGridMessage
            {
                From = from,
                Subject = $"Contact form: {subject}",
                HtmlContent = model.Message
            };
            msg.AddTo(new EmailAddress(EmailConstants.AdminEmailAddress));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
