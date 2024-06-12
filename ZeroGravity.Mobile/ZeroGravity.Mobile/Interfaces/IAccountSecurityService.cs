using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IAccountSecurityService
    {
        Task<ApiCallResult<string>> ChangeEmail(string newEmail, CancellationToken cancellationToken);
        Task<ApiCallResult<string>> ChangePassword(string oldPassword, string newPassword, CancellationToken cancellationToken);
        Task<PasswordConfirmResult> ConfirmPassword(string password, CancellationToken cancellationToken);
        Task<ApiCallResult<string>> DeleteAccount(CancellationToken cancellationToken);
    }
}
