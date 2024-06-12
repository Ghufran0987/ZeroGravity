using System;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Enums;

namespace ZeroGravity.Mobile.Base.Interfaces
{
    /// <summary>
    /// Provider for the ViewModels
    /// </summary>
    public interface IPageVmProvider
    {
        Task<TokenStatus> GetTokenStatus();
        Task SaveNewJwt(string jwt);
        Task SaveNewRefreshToken(string newRefreshToken, DateTime newRefreshTokenExpiration);
    }
}
