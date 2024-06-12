using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IAccountDeletePageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<bool>> ConfirmPassword(string password, CancellationToken cancellationToken);
        Task<ApiCallResult<string>> DeleteAccount(CancellationToken cancellationToken);
    }
}
