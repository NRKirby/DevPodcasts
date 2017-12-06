using System.ComponentModel.DataAnnotations;

namespace DevPodcasts.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Keep me logged in")]
        public bool RememberMe { get; set; }
    }
}