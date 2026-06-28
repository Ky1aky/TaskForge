using System.ComponentModel.DataAnnotations;

namespace TaskForge.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Member Since")]
        public DateTime DateCreated { get; set; }
    }
}