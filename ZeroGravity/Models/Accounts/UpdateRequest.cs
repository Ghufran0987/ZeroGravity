using System.ComponentModel.DataAnnotations;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Models.Accounts
{
    public class UpdateRequest
    {
        private string _password;
        private string _confirmPassword;
        private string _role;
        private string _email;
        
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool WantsNewsletter { get; set; }

        [EnumDataType(typeof(Role))]
        public string Role
        {
            get => _role;
            set => _role = ReplaceEmptyWithNull(value);
        }

        [EmailAddress]
        public string Email
        {
            get => _email;
            set => _email = ReplaceEmptyWithNull(value);
        }

        [MinLength(6)]
        public string Password
        {
            get => _password;
            set => _password = ReplaceEmptyWithNull(value);
        }

        [Compare("Password")]
        public string ConfirmPassword 
        {
            get => _confirmPassword;
            set => _confirmPassword = ReplaceEmptyWithNull(value);
        }

        public UnitDisplayType UnitDisplayType { get; set; }
        public DateTimeDisplayType DateTimeDisplayType { get; set; }

        // helpers

        private string ReplaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}