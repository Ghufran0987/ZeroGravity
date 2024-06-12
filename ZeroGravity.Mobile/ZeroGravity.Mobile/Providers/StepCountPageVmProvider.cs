using System;
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
    public class StepCountPageVmProvider : PageVmProviderBase, IStepCountPageVmProvider
    {

        private readonly IStepCounterService _stepCounterService;
        private readonly IStepCountDataService _countDataService;
        private readonly ILogger _logger;

        public StepCountPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IStepCounterService stepCounterService, IStepCountDataService countDataService) : base(tokenService)
        {
            _stepCounterService = stepCounterService;
            _countDataService = countDataService;
            _logger = loggerFactory?.CreateLogger<StepCountPageVmProvider>() ??
                      new NullLogger<StepCountPageVmProvider>();
        }

        public void InitSensorService()
        {
            _stepCounterService.InitSensorService();
        }

        public void StopSensorService()
        {
            _stepCounterService.StopSensorService();
        }

        public int GetCurrentSteps()
        {
            return _stepCounterService.GetSteps();
        }

        public bool IsSensorAvailable()
        {
            return _stepCounterService.IsAvailable();
        }

        public async Task<ApiCallResult<StepCountDataProxy>> GetStepCountDataAsync(int stepCountId, CancellationToken cancellationToken)
        {
            var apiCallResult = await _countDataService.GetStepCountDataAsync(stepCountId, cancellationToken);

            if (apiCallResult.Success)
            {
                var stepCountDataProxy = ProxyConverter.GetStepCountDataProxy(apiCallResult.Value);

                return ApiCallResult<StepCountDataProxy>.Ok(stepCountDataProxy);
            }

            return ApiCallResult<StepCountDataProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<StepCountDataProxy>> GetStepCountDataByDateAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _countDataService.GetStepCountDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var stepCountDataProxy = ProxyConverter.GetStepCountDataProxy(apiCallResult.Value);

                return ApiCallResult<StepCountDataProxy>.Ok(stepCountDataProxy);
            }

            return ApiCallResult<StepCountDataProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<StepCountDataProxy>> CreateStepCountDataAsync(StepCountDataProxy stepCountDataProxy, CancellationToken cancellationToken)
        {
            var stepCountDataDto = ProxyConverter.GetStepCountDataDto(stepCountDataProxy);

            var apiCallResult = await _countDataService.CreateStepCountDataAsync(stepCountDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                stepCountDataProxy = ProxyConverter.GetStepCountDataProxy(apiCallResult.Value);

                return ApiCallResult<StepCountDataProxy>.Ok(stepCountDataProxy);
            }

            return ApiCallResult<StepCountDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<StepCountDataProxy>> UpdateStepCountDataAsync(StepCountDataProxy stepCountDataProxy, CancellationToken cancellationToken)
        {
            var stepCountDataDto = ProxyConverter.GetStepCountDataDto(stepCountDataProxy);

            var apiCallResult = await _countDataService.UpdateStepCountDataAsync(stepCountDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                stepCountDataProxy = ProxyConverter.GetStepCountDataProxy(apiCallResult.Value);

                return ApiCallResult<StepCountDataProxy>.Ok(stepCountDataProxy);
            }

            return ApiCallResult<StepCountDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}