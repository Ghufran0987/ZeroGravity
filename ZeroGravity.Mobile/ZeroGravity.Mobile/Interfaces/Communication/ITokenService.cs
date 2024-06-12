using System.Threading.Tasks;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Interfaces.Communication
{
    public interface ITokenService
    {
        Task AddOrUpdateJwt(string jwt);
        Task AddOrUpdateRefreshToken(RefreshToken refreshToken);

        Task<bool> IsJwtExpiredOrInvalid();
        Task<bool> IsRefreshTokenExpired();


        Task RemoveJwt();
        Task RemoveRefreshToken();

        Task<string> GetJsonWebToken();
        Task<RefreshToken> GetRefreshToken();

        Task<bool> AreBothTokensNotExisting();
    }
}
