namespace ZeroGravity.Shared.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool AcceptTerms { get; set; }
        public bool WantsNewsletter { get; set; }
    }
}
