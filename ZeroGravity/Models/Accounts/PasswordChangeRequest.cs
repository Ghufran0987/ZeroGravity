using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class PasswordChangeRequest
    {
        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPasswordConfirm { get; set; }

        [Required]
        [MinLength(6)]
        public string OldPassword { get; set; }
    }
}
