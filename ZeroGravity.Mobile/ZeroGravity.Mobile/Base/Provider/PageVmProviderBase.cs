using System;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Base.Provider
{
    public abstract class PageVmProviderBase : IPageVmProvider
    {
        private readonly ITokenService _tokenService;

        //protected PageVmProviderBase()
        //{
            
        //}

        protected PageVmProviderBase(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenStatus> GetTokenStatus()
        {
            if (_tokenService == null)
            {
                return TokenStatus.Other;
            }

            if (await _tokenService.AreBothTokensNotExisting())
            {
                // either user is first time using the app
                // or has previously logged out
                // or has delete app storage/cache
                return TokenStatus.BothNotExisting;
            }

            if (!await _tokenService.IsJwtExpiredOrInvalid())
            {
                // jwt is valid and not expired
                return TokenStatus.JwtValid;
            }

            // remove token from SecureStorage
            await _tokenService.RemoveJwt();

            if (!await _tokenService.IsRefreshTokenExpired())
            {
                // refresh token is valid
                return TokenStatus.RefreshTokenValid;
            }

            // remove refreshToken from SecureStorage
            await _tokenService.RemoveRefreshToken();

            return TokenStatus.Invalid;
        }

        public async Task SaveNewJwt(string jwt)
        {
            await _tokenService.AddOrUpdateJwt(jwt);
        }

        public async Task SaveNewRefreshToken(string newRefreshToken, DateTime newRefreshTokenExpiration)
        {
            var refreshToken = new RefreshToken
            {
                Token = newRefreshToken,
                Expiration = newRefreshTokenExpiration
            };

            await _tokenService.AddOrUpdateRefreshToken(refreshToken);
        }
    }
}
