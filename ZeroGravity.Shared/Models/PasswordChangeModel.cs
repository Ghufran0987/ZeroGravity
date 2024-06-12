namespace ZeroGravity.Shared.Models
{
    public class PasswordChangeModel
    {
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
        public string OldPassword { get; set; }
    }
}
