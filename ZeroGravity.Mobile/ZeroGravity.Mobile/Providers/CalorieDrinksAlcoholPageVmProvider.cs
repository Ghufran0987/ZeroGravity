using System;
using System.Collections.Generic;
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
    public class CalorieDrinksAlcoholPageVmProvider : PageVmProviderBase, ICalorieDrinksAlcoholPageVmProvider
    {
        private readonly ILiquidIntakeDataService _liquidIntakeDataService;
        private readonly IFastingDataService _fastingDataService;
        private ILogger _logger;

        public CalorieDrinksAlcoholPageVmProvider(ITokenService tokenService, ILoggerFactory loggerFactory, 
            ILiquidIntakeDataService liquidIntakeDataService, IFastingDataService fastingDataService) : base(tokenService)
        {
            _liquidIntakeDataService = liquidIntakeDataService;
            _fastingDataService = fastingDataService;
            _logger = loggerFactory?.CreateLogger<CalorieDrinksAlcoholPageVmProvider>() ??
                      new NullLogger<CalorieDrinksAlcoholPageVmProvider>();
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> GetLiquidIntakeDataAsync(int activityId, CancellationToken cancellationToken)
        {
            var apiCallResult = await _liquidIntakeDataService.GetLiquidIntakeDataAsync(activityId, cancellationToken);

            if (apiCallResult.Success)
            {
                var liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

                return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            }

            return ApiCallResult<LiquidIntakeDataProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> GetLiquidIntakeDataByDateAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            return null;
            //var apiCallResult = await _liquidIntakeDataService.GetLiquidIntakeDataByDateAsync(dateTime, cancellationToken);

            //if (apiCallResult.Success)
            //{
            //    var liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

            //    return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            //}

            //return ApiCallResult<LiquidIntakeDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> CreateLiquidIntakeDataAsync(LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken)
        {
            var liquidIntakeDataDto = ProxyConverter.GetLiquidIntakeDataDto(liquidIntakeDataProxy);

            var apiCallResult = await _liquidIntakeDataService.CreateLiquidIntakeDataAsync(liquidIntakeDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

                return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            }

            return ApiCallResult<LiquidIntakeDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<LiquidIntakeDataProxy>> UpdateLiquidIntakeDataAsync(LiquidIntakeDataProxy liquidIntakeDataProxy, CancellationToken cancellationToken)
        {
            var liquidIntakeDataDto = ProxyConverter.GetLiquidIntakeDataDto(liquidIntakeDataProxy);

            var apiCallResult = await _liquidIntakeDataService.UpdateLiquidIntakeDataAsync(liquidIntakeDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                liquidIntakeDataProxy = ProxyConverter.GetLiquidIntakeDataProxy(apiCallResult.Value);

                return ApiCallResult<LiquidIntakeDataProxy>.Ok(liquidIntakeDataProxy);
            }

            return ApiCallResult<LiquidIntakeDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<IEnumerable<FastingDataProxy>>> GetActiveFastingDataAsync(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _fastingDataService.GetActiveFastingDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var proxies = new List<FastingDataProxy>();
                foreach (var fastingDataDto in apiCallResult.Value)
                {
                    var fastingDataProxy = ProxyConverter.GetFastingDataProxy(fastingDataDto);
                    proxies.Add(fastingDataProxy);
                }
                
                return ApiCallResult<IEnumerable<FastingDataProxy>>.Ok(proxies);
            }

            return ApiCallResult<IEnumerable<FastingDataProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}