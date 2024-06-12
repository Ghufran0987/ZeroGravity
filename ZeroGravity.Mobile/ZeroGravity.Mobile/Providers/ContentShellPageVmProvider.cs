
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class ContentShellPageVmProvider : PageVmProviderBase, IContentShellPageVmProvider
    {
        private readonly ILogger _logger;
        private readonly ILoginService _loginService;
        private readonly IApiService _apiService;
        private readonly IUserDataService _userDataService;
        private readonly ITokenService _tokenService;


        public ContentShellPageVmProvider(ILoggerFactory loggerFactory, ILoginService loginService, ITokenService tokenService, IApiService apiService, IUserDataService userDataService) : base(tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _apiService = apiService;
            _userDataService = userDataService;
            _logger = loggerFactory?.CreateLogger<ContentShellPageVmProvider>() ?? new NullLogger<ContentShellPageVmProvider>();
        }

        public void SetDisplayPreferences(AccountResponseDto accountResponseDto)
        {
            DisplayConversionService.SetDisplayPrefences(accountResponseDto.DateTimeDisplayType, accountResponseDto.UnitDisplayType);
        }

        public async Task<AccountResponseDto> GetAccountDataAsync()
        {
            CancellationToken cancellationToken = new CancellationToken();

            var apiCallResult = await _userDataService.GetAccountDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                return apiCallResult.Value;
            }

            return null;
        } 

        public async Task<LoginResult> TrySilentLogin()
        {
            return await _loginService.LoginSilent();
        }

        public async Task RefreshTokenAsync()
        {
            // get new JwtToken using refresh-token api
            var baseUrl = Common.ServerUrl;
            var api = "/accounts/refresh-token";
            var url = baseUrl + api;

            var response = await _apiService.PostRx<LoginResponse>(url, CancellationToken.None);
            await _tokenService.AddOrUpdateJwt(response.JwtToken);
        }

        public async Task<bool> IsJwtExpiredOrInvalid()
        {
           return await _tokenService.IsJwtExpiredOrInvalid();
        }
    }
}
