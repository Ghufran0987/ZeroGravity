using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}