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
    public class MedicalPreConditionsPageVmProvider : PageVmProviderBase, IMedicalPreConditionsPageVmProvider
    {
        private readonly ILogger _logger;

        private readonly IUserDataService _userDataService;

        public MedicalPreConditionsPageVmProvider(ILoggerFactory loggerFactory, IUserDataService userDataService, ITokenService tokenService) : base(tokenService)
        {
            _userDataService = userDataService;
            _logger = loggerFactory?.CreateLogger<MedicalPreConditionsPageVmProvider>() ??
                      new NullLogger<MedicalPreConditionsPageVmProvider>();
        }


        public async Task<ApiCallResult<MedicalPreconditionsProxy>> GetMedicalPreConditionsAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetMedicalPreConditionAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var personalDataProxy = ProxyConverter.GetMedicalConditionProxy(apiCallResult.Value);

                return ApiCallResult<MedicalPreconditionsProxy>.Ok(personalDataProxy);
            }

            return ApiCallResult<MedicalPreconditionsProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<MedicalPreconditionsProxy>> CreateMedicalConditionsAsnyc(MedicalPreconditionsProxy medicalPreconditionsProxy, CancellationToken cancellationToken)
        {
            var medicalConditionDto = ProxyConverter.GetMedicalConditionDto(medicalPreconditionsProxy);

            var apiCallResult = await _userDataService.CreateMedicalPreConditionAsync(medicalConditionDto, cancellationToken);

            if (apiCallResult.Success)
            {
                medicalPreconditionsProxy = ProxyConverter.GetMedicalConditionProxy(apiCallResult.Value);

                return ApiCallResult<MedicalPreconditionsProxy>.Ok(medicalPreconditionsProxy);
            }

            return ApiCallResult<MedicalPreconditionsProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<MedicalPreconditionsProxy>> UpdateMedicalConditionsAsnyc(MedicalPreconditionsProxy medicalPreconditionsProxy, CancellationToken cancellationToken)
        {
            var medicalConditionDto = ProxyConverter.GetMedicalConditionDto(medicalPreconditionsProxy);

            var apiCallResult = await _userDataService.UpdateMedicalPreConditionAsync(medicalConditionDto, cancellationToken);

            if (apiCallResult.Success)
            {
                medicalPreconditionsProxy = ProxyConverter.GetMedicalConditionProxy(apiCallResult.Value);

                return ApiCallResult<MedicalPreconditionsProxy>.Ok(medicalPreconditionsProxy);
            }

            return ApiCallResult<MedicalPreconditionsProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}