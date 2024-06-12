using System;
using System.Linq;
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
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Providers
{
    public class WaterIntakePageVmProvider : PageVmProviderBase, IWaterIntakePageVmProvider
    {
        private readonly ILiquidIntakeDataService _liquidIntakeDataService;
        private ILogger _logger;

        public WaterIntakePageVmProvider(ITokenService tokenService, ILoggerFactory loggerFactory,
            ILiquidIntakeDataService liquidIntakeDataService) : base(tokenService)
        {
            _liquidIntakeDataService = liquidIntakeDataService;
            _logger = loggerFactory?.CreateLogger<WaterIntakePageVmProvider>() ??
                      new NullLogger<WaterIntakePageVmProvider>();
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> GetLiquidIntakeDataAsnyc(int activityId,
            CancellationToken cancellationToken)
        {
            var apiCallResult = await _liquidIntakeDataService.GetLiquidIntakeDataAsync(activityId, cancellationToken);

            if (apiCallResult.Success)
            {
                var liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

                return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            }

            return ApiCallResult<LiquidIntakeDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<double>> GetLiquidIntakeDataSummaryAsync(DateTime dateTime, LiquidType type,
            CancellationToken cancellationToken)
        {
            var apiCallResult =
                await _liquidIntakeDataService.GetLiquidIntakeDataByDateAsync(dateTime, type, cancellationToken);

            if (apiCallResult.Success)
            {
                var summary = apiCallResult.Value.Select(ProxyConverter.GetLiquidIntakeDataProxy).Sum(_ => _.AmountMl);

                return ApiCallResult<double>.Ok(summary);
            }

            return ApiCallResult<double>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> CreateLiquidIntakeDataAsnyc(
            LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken)
        {
            var liquidIntakeDataDto = ProxyConverter.GetLiquidIntakeDataDto(liquidIntakeDataProxy);

            var apiCallResult =
                await _liquidIntakeDataService.CreateLiquidIntakeDataAsync(liquidIntakeDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

                return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            }

            return ApiCallResult<LiquidIntakeDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> UpdateLiquidIntakeDataAsnyc(
            LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken)
        {
            var liquidIntakeDataDto = ProxyConverter.GetLiquidIntakeDataDto(liquidIntakeDataProxy);

            var apiCallResult =
                await _liquidIntakeDataService.UpdateLiquidIntakeDataAsync(liquidIntakeDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

                return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            }

            return ApiCallResult<LiquidIntakeDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}