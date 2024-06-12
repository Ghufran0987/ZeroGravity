using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}