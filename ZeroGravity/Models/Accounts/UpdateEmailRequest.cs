using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Models.Accounts
{
    public class UpdateEmailRequest
    {
        [EmailAddress]
        public string NewEmail { get; set; }
    }
}
