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
    public class FastingDataPageVmProvider : PageVmProviderBase, IFastingDataPageVmProvider
    {
        private readonly IFastingDataService _fastingDataService;
        private readonly ILogger _logger;

        public FastingDataPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IFastingDataService fastingDataService) : base(tokenService)
        {
            _fastingDataService = fastingDataService;
            _logger = loggerFactory?.CreateLogger<FastingDataPageVmProvider>() ??
                      new NullLogger<FastingDataPageVmProvider>();
        }

        public async Task<ApiCallResult<FastingDataProxy>> GetFastingDataByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _fastingDataService.GetFastingDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var fastingDataProxy = ProxyConverter.GetFastingDataProxy(apiCallResult.Value);

                return ApiCallResult<FastingDataProxy>.Ok(fastingDataProxy);
            }

            return ApiCallResult<FastingDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<FastingDataProxy>> CreateFastingDataAsnyc(FastingDataProxy fastingDataProxy, CancellationToken cancellationToken)
        {
            var fastingDataDto = ProxyConverter.GetFastingDataDto(fastingDataProxy);

            var apiCallResult = await _fastingDataService.CreateFastingDataAsync(fastingDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                fastingDataProxy = ProxyConverter.GetFastingDataProxy(apiCallResult.Value);

                return ApiCallResult<FastingDataProxy>.Ok(fastingDataProxy);
            }

            return ApiCallResult<FastingDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<FastingDataProxy>> UpdateFastingDataAsync(FastingDataProxy fastingDataProxy, CancellationToken cancellationToken)
        {
            var fastingDataDto = ProxyConverter.GetFastingDataDto(fastingDataProxy);

            var apiCallResult = await _fastingDataService.UpdateFastingDataAsync(fastingDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                fastingDataProxy = ProxyConverter.GetFastingDataProxy(apiCallResult.Value);

                return ApiCallResult<FastingDataProxy>.Ok(fastingDataProxy);
            }

            return ApiCallResult<FastingDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}