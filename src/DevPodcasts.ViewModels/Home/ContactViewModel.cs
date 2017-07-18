using DevPodcasts.Models;
using System.ComponentModel.DataAnnotations;

namespace DevPodcasts.ViewModels.Home
{
    public class ContactViewModel
    {
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        public string GCaptchaResponse { get; set; }

        public bool? IsSuccess { get; set; }

        public string SuccessMessage { get; set; }
    }
}
