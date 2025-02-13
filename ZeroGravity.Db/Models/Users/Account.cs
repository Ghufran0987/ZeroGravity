using System;
using System.Collections.Generic;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Models.Users
{
    public class Account: ModelBase
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
        public string PasswordHash { get; set; }
        public bool AcceptTerms { get; set; }
        public byte[] Image { get; set; }
        public Role Role { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? OnBoardingDate { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool CompletedFirstUseWizard { get; set; }
        public bool WantsNewsletter { get; set; }
        public UnitDisplayType UnitDisplayType { get; set; }
        public DateTimeDisplayType DateTimeDisplayType { get; set; }

        public bool OwnsToken(string token) 
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}