using DevPodcasts.Models;
using DevPodcasts.ViewModels.Home;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;
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
                model.SuccessMessage = GetSuccessMessage(model.Subject);
            }
            else
            {
                model.IsSuccess = false;
            }

            return model;
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
            var subject = model.Subject.GetAttribute<DisplayAttribute>().Name;
            var msg = new SendGridMessage
            {
                From = from,
                Subject = $"Contact form: {subject}",
                HtmlContent = model.Message
            };
            msg.AddTo(new EmailAddress(EmailConstants.AdminEmailAddress));
            var response = await client.SendEmailAsync(msg);
        }

        private string GetSuccessMessage(ContactSubject subject)
        {
            string result;
            switch (subject)
            {
                case ContactSubject.Feedback:
                    result = "Thanks for your feedback!";
                    break;
                case ContactSubject.ReportAnIssue:
                    result = "Thanks for contacting us, we will get back to you as soon as we can.";
                    break;
                case ContactSubject.ReportABug:
                    result = "Thanks for letting us know, we will contact you if we need more information.";
                    break;
                case ContactSubject.SuggestAFeature:
                    result = "Thanks for the suggestion, we are always trying to improve the site!";
                    break;

                default:
                    result = string.Empty;
                    break;

            }
            return result;
        }
    }
}
