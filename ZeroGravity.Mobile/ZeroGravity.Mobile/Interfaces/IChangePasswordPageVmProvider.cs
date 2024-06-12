using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IChangePasswordPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<bool>> ConfirmPassword(string password, CancellationToken cancellationToken);
        Task<ApiCallResult<string>> ChangePassword(string oldPassword, string newPassword, CancellationToken cancellationToken);
    }
}
