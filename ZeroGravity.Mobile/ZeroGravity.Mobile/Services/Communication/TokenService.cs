using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Services.Communication
{
    public class TokenService : ITokenService
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILogger _logger;

        public TokenService(ILoggerFactory loggerFactory, ISecureStorageService secureStorageService/*, IApiService apiService*/)
        {
            _secureStorageService = secureStorageService;

            _logger = loggerFactory?.CreateLogger<TokenService>() ?? new NullLogger<TokenService>();
        }

        public async Task<bool> AreBothTokensNotExisting()
        {
            var jwt = await _secureStorageService.LoadString(SecureStorageKey.JsonWebToken);
            var refresh = await _secureStorageService.LoadString(SecureStorageKey.RefreshToken);

            if (jwt == null && refresh == null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> IsJwtExpiredOrInvalid()
        {
            var jwt = await _secureStorageService.LoadString(SecureStorageKey.JsonWebToken);

            if (jwt == null)
            {
                return true;
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var tokenExpiryDate = token.ValidTo;

            var idClaim = token.Claims?.FirstOrDefault(x => x.Type == "id");
            var id = idClaim?.Value;

            // If there is no valid `exp` claim then `ValidTo` returns DateTime.MinValue
            if (tokenExpiryDate == DateTime.MinValue)
            {
                _logger.LogInformation($"Could not get exp claim from token for userId: {id}.");
                return true;
            }

            // If the token is in the past then you can't use it
            if (tokenExpiryDate < DateTime.UtcNow)
            {
                _logger.LogInformation($"Token for userId: {id} expired on: {tokenExpiryDate}");
                return true;
            }

            return false;
        }

        public async Task AddOrUpdateJwt(string jwt)
        {
            await _secureStorageService.Remove(SecureStorageKey.JsonWebToken);
            await _secureStorageService.SaveString(SecureStorageKey.JsonWebToken, jwt);
            _logger.LogDebug($"Added a new Jwt: {jwt} to device storage");
        }

        public async Task AddOrUpdateRefreshToken(RefreshToken refreshToken)
        {
            await _secureStorageService.Remove(SecureStorageKey.RefreshToken);
            await _secureStorageService.SaveObject(SecureStorageKey.RefreshToken, refreshToken);
            _logger.LogDebug($"Added a new refreshToken: {refreshToken.Token} to device storage");

        }

        public async Task RemoveJwt()
        {
            var jwt = await GetJsonWebToken();

            await _secureStorageService.Remove(SecureStorageKey.JsonWebToken);

            _logger.LogDebug($"Removed the Jwt: {jwt} from device storage");
        }

        public async Task RemoveRefreshToken()
        {
            var refresh = await GetRefreshToken();

            await _secureStorageService.Remove(SecureStorageKey.RefreshToken);

            _logger.LogDebug($"Removed the refreshToken: {refresh?.Token} from device storage");
        }

        public async Task<bool> IsRefreshTokenExpired()
        {
            var refreshToken = await _secureStorageService.LoadObject<RefreshToken>(SecureStorageKey.RefreshToken);

            if (refreshToken == null)
            {
                return true;
            }

            // If the token is in the past then you can't use it
            if (refreshToken.Expiration < DateTime.UtcNow)
            {
                _logger.LogInformation($"RefreshToken {refreshToken.Token} expired at {refreshToken.Expiration}");
                return true;
            }

            return false;
        }

        public async Task<RefreshToken> GetRefreshToken()
        {
            return await _secureStorageService.LoadObject<RefreshToken>(SecureStorageKey.RefreshToken);
        }

        public async Task<string> GetJsonWebToken()
        {
            return await _secureStorageService.LoadString(SecureStorageKey.JsonWebToken);
        }
    }
}
