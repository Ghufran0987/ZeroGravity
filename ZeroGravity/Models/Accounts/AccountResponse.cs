using System;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Models.Accounts
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? Verified { get; set; }
        public bool CompletedFirstUseWizard { get; set; }
        public DateTime? OnBoardingDate { get; set; }
        public UnitDisplayType UnitDisplayType { get; set; }
        public DateTimeDisplayType DateTimeDisplayType { get; set; }
    }
}