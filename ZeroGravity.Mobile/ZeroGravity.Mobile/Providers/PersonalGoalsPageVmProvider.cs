using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class PersonalGoalsPageVmProvider : PageVmProviderBase, IPersonalGoalsPageVmProvider
    {
        private readonly IUserDataService _userDataService;
        private readonly ILogger _logger;

        public PersonalGoalsPageVmProvider(ILoggerFactory loggerFactory, IUserDataService userDataService, ITokenService tokenService) : base(tokenService)
        {
            _userDataService = userDataService;

            _logger = loggerFactory?.CreateLogger<PersonalGoalsPageVmProvider>() ??
                      new NullLogger<PersonalGoalsPageVmProvider>();
        }

        public async Task<ApiCallResult<PersonalDataProxy>> GetPersonalDataAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult =  await _userDataService.GetPersonalDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var personalDataProxy = ProxyConverter.GetPersonalDataProxy(apiCallResult.Value);

                return ApiCallResult<PersonalDataProxy>.Ok(personalDataProxy);
            }

            return ApiCallResult<PersonalDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> GetPersonalGoalAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetPersonalGoalAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> CreatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken)
        {
            var personalGoalDto = ProxyConverter.GetPersonalGoalDto(personalGoalProxy);

            var apiCallResult = await _userDataService.CreatePersonalGoalAsync(personalGoalDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> UpdatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken)
        {
            var personalGoalDto = ProxyConverter.GetPersonalGoalDto(personalGoalProxy);

            var apiCallResult = await _userDataService.UpdatePersonalGoalAsync(personalGoalDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}