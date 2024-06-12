using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IFirstUseWizardPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsnyc(CancellationToken token);

        Task<ApiCallResult<bool>> UpdateAccountDataAsnyc(UpdateWizardRequestDto updateWizardRequestDto, CancellationToken cancellationToken);
    }
}