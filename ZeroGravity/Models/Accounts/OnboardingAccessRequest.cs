using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class OnboardingAccessRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}