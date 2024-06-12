using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class VerifyOnboardingAccessRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}