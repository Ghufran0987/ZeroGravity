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
    public class WellbeingDataPageVmProvider : PageVmProviderBase, IWellbeingDataPageVmProvider
    {
        private readonly IWellbeingDataService _wellbeingDataService;
        private readonly ILogger _logger;

        public WellbeingDataPageVmProvider(ILoggerFactory loggerFactory, IWellbeingDataService wellbeingDataService, ITokenService tokenService) : base(tokenService)
        {
            _wellbeingDataService = wellbeingDataService;
            _logger = loggerFactory?.CreateLogger<WellbeingDataPageVmProvider>() ??
                      new NullLogger<WellbeingDataPageVmProvider>();
        }

        public async Task<ApiCallResult<WellbeingDataProxy>> GetDayToDayActivityAsnyc(int activityId, CancellationToken cancellationToken)
        {
            var apiCallResult = await _wellbeingDataService.GetWellbeingDataAsync(activityId, cancellationToken);

            if (apiCallResult.Success)
            {
                var dayToDayActivityProxy = ProxyConverter.GetWellbeingDataProxy(apiCallResult.Value);

                return ApiCallResult<WellbeingDataProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<WellbeingDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<WellbeingDataProxy>> GetWellbeingDataByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _wellbeingDataService.GetWellbeingDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var wellbeingDataProxy = ProxyConverter.GetWellbeingDataProxy(apiCallResult.Value);

                return ApiCallResult<WellbeingDataProxy>.Ok(wellbeingDataProxy);
            }

            return ApiCallResult<WellbeingDataProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<WellbeingDataProxy>> CreatWellbeingDataAsnyc(WellbeingDataProxy wellbeingDataProxy, CancellationToken cancellationToken)
        {
            var wellbeingDataDto = ProxyConverter.GetWellbeingDataDto(wellbeingDataProxy);

            var apiCallResult = await _wellbeingDataService.CreateWellbeingDataAsync(wellbeingDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                wellbeingDataProxy = ProxyConverter.GetWellbeingDataProxy(apiCallResult.Value);

                return ApiCallResult<WellbeingDataProxy>.Ok(wellbeingDataProxy);
            }

            return ApiCallResult<WellbeingDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<WellbeingDataProxy>> UpdateWellbeingDataAsnyc(WellbeingDataProxy wellbeingDataProxy, CancellationToken cancellationToken)
        {
            var wellbeingDataDto = ProxyConverter.GetWellbeingDataDto(wellbeingDataProxy);

            var apiCallResult = await _wellbeingDataService.UpdateWellbeingDataAsync(wellbeingDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                wellbeingDataProxy = ProxyConverter.GetWellbeingDataProxy(apiCallResult.Value);

                return ApiCallResult<WellbeingDataProxy>.Ok(wellbeingDataProxy);
            }

            return ApiCallResult<WellbeingDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}