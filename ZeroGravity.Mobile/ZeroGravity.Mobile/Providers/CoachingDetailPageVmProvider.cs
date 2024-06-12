using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class CoachingDetailPageVmProvider : PageVmProviderBase, ICoachingDetailPageVmProvider
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IUserDataService _userDataService;
        private readonly IApiService _api;

        public CoachingDetailPageVmProvider(ITokenService tokenService, IApiService api, ISecureStorageService secureStorageService, IUserDataService userDataService) : base(tokenService)
        {
            _api = api;
            _secureStorageService = secureStorageService;
            _userDataService = userDataService;
        }

        public async Task<string> GetCurrentUserEmailAsync(CancellationToken cancellationToken)
        {
            var userEmail = await _secureStorageService.LoadString(SecureStorageKey.UserEmail);

            if (string.IsNullOrEmpty(userEmail))
            {
                // try get from server
                var apiCall = await GetAccountDataAsync(cancellationToken);
                if (apiCall.Success)
                {
                    return apiCall.Value.Email;
                }

                return string.Empty;
            }

            return userEmail;
        }

        public async Task<bool> SubmitInterestAsync(int userId, string email, CoachingType type, List<string> options, CancellationToken token)
        {
            var request = new CoachingInterestModel
            {
                UserId = userId,
                Coaching = type,
                Email = email,
                CoachingOptions = options
            };

            var baseUrl = Common.ServerUrl;
            var api = "/feedback/coaching/interest";
            var url = baseUrl + api;

            var result = await _api.PostJsonAsyncRx<CoachingInterestModel, bool>(url, request, token);

            return result;
        }

        private async Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsync(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetAccountDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}