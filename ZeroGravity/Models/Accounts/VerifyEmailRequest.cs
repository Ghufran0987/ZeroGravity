using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}