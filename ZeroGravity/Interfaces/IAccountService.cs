using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.Users;
using ZeroGravity.Models.Accounts;

namespace ZeroGravity.Interfaces
{
    public interface IAccountService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        void Register(RegisterRequest model, string origin);
        void VerifyEmail(string token, bool isEmailChangeRequest = false);
        void ForgotPassword(ForgotPasswordRequest model, string origin);
        void ValidateResetToken(string token);
        void ResetPassword(string token, string email, string newPassword);
        bool ConfirmPassword(PasswordConfirmRequest model);
        IEnumerable<AccountResponse> GetAll();
        AccountResponse GetById(int id);
        AccountResponse Create(CreateRequest model);
        AccountResponse Update(int id, UpdateRequest model);
        bool UpdateWizardState(int id, UpdateWizardRequest model);
        void Delete(int id);
        void NewEmailRequest(int id, UpdateEmailRequest model, string origin);
        void UpdateEmail(string token);
        void UpdatePassword(int id, PasswordChangeRequest model);

        Task<Account> UploadProfileImage(int id, ProfileImage profileImage);
        ProfileImage GetProfileImage(int id);

        Task<AppInfoData> StoreAppInfo(AppInfoData appInfoData);
        Task<PushNotificationTokenData> StorePushNotificationToken(PushNotificationTokenData pushNotificationTokenData);
        Task<UserQueryData> StoreUserQuery(UserQueryData userQueryData);

        OnboardingAccessResponse RequestOnboardingAccess(OnboardingAccessRequest request);
        OnboardingAccessResponse VerifyOnboardingAccess(VerifyOnboardingAccessRequest request);
    }
}