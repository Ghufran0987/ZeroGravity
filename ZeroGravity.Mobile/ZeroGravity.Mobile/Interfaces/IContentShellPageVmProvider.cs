using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IContentShellPageVmProvider : IPageVmProvider
    {
        Task<LoginResult> TrySilentLogin();
        Task RefreshTokenAsync();
        void SetDisplayPreferences(AccountResponseDto accountResponseDto);
        Task<AccountResponseDto> GetAccountDataAsync();
        Task<bool> IsJwtExpiredOrInvalid();
    }
}