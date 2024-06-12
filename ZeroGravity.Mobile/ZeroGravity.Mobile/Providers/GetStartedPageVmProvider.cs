using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class GetStartedPageVmProvider : PageVmProviderBase, IGetStartedPageVmProvider
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILoginService _loginService;
        private readonly IUserDataService _userDataService;
        private readonly ILogger _logger;

        public GetStartedPageVmProvider(ISecureStorageService secureStorageService, ILoggerFactory loggerFactory,
        ITokenService tokenService, ILoginService loginService, IUserDataService userDataService) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<GetStartedPageVmProvider>() ?? new NullLogger<GetStartedPageVmProvider>();
            _secureStorageService = secureStorageService;
            _loginService = loginService;
            _userDataService = userDataService;
            
        }
   


        public async Task<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken, bool saveToken = false)
        {
            return await _loginService.LoginAsync(email, password, cancellationToken, saveToken);
        }

        public async Task<LoginProxy> GetLoginProxy()
        {
            var email = string.Empty;
            var password = string.Empty;
            var saveLogin = false;

            try
            {
                var loginModel = await _secureStorageService.LoadObject<LoginModel>(SecureStorageKey.LoginData);
                if (loginModel != null)
                {
                    email = loginModel.Email;
                    password = loginModel.Password;
                }
                else
                {
#if DEBUG
                    //email = "zerog@tzm.de";
                    //password = "Admin1!";
                    //saveLogin = true;
#endif
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, e.Message);
            }

            return new LoginProxy
            {
                Email = email,
                Password = password,
                SaveLogin = saveLogin
            };
        }

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsnyc(CancellationToken token)
        {
            var apiCallResult = await _userDataService.GetAccountDataAsync(token);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage);
        }

        public Task SaveCredentials(bool saveLogin, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
