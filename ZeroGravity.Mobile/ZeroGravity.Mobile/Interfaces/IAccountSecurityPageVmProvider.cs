using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IAccountSecurityPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken cancellationToken);
        Task<string> GetUserEmailFromSecureStorageAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}