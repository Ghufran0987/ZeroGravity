using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class MyWeightPageVmProvider : PageVmProviderBase, IMyWeightPageVmProvider
    {
        private readonly IWeightDataService _weightDataService;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ITokenService _tokenService;

        public MyWeightPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IWeightDataService weightDataService) : base(tokenService)
        {
            _weightDataService = weightDataService;
            _loggerFactory = loggerFactory;
            _tokenService = tokenService;
        }

        public async Task<ApiCallResult<WeightItemProxy>> AddWeightDataAsync(WeightItemProxy proxy, CancellationToken cancellationToken)
        {
            var dto = ProxyConverter.GetWeightDto(proxy);
            var result = await _weightDataService.AddWeightDataAsync(dto, cancellationToken);
            return result.Success
                ? ApiCallResult<WeightItemProxy>.Ok(ProxyConverter.GetWeightItemProxy(result.Value))
                : ApiCallResult<WeightItemProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<WeightItemDetailsProxy>> AddWeightDetailsDataAsync(WeightItemDetailsProxy proxy, CancellationToken cancellationToken)
        {
            var dto = ProxyConverter.GetWeightDetailsDto(proxy);
            var result = await _weightDataService.AddWeightDetailsDataAsync(dto, cancellationToken);
            return result.Success
                ? ApiCallResult<WeightItemDetailsProxy>.Ok(ProxyConverter.GetWeightItemDetailsProxy(result.Value))
                : ApiCallResult<WeightItemDetailsProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<List<WeightItemProxy>>> GetAll(int accountId, CancellationToken cancellationToken)
        {
            var result = await _weightDataService.GetAll(accountId, cancellationToken);
            return result.Success
                ? ApiCallResult<List<WeightItemProxy>>.Ok(ProxyConverter.GetWeightItemProxies(result.Value))
                : ApiCallResult<List<WeightItemProxy>>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<WeightItemDetailsProxy>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _weightDataService.GetByIdAsync(id, cancellationToken);
            return result.Success
                ? ApiCallResult<WeightItemDetailsProxy>.Ok(ProxyConverter.GetWeightItemDetailsProxy(result.Value))
                : ApiCallResult<WeightItemDetailsProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<WeightItemDetailsProxy>> GetCurrentWeightTrackerAsync(CancellationToken cancellationToken)
        {
            var result = await _weightDataService.GetCurrentWeightTrackerAsync(cancellationToken);
            return result.Success
                ? ApiCallResult<WeightItemDetailsProxy>.Ok(ProxyConverter.GetWeightItemDetailsProxy(result.Value))
                : ApiCallResult<WeightItemDetailsProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<WeightItemDetailsProxy>> UpdateAsync(WeightItemDetailsProxy proxy, CancellationToken cancellationToken)
        {
            var dto = ProxyConverter.GetWeightDetailsDto(proxy);

            var result = await _weightDataService.UpdateAsync(dto, cancellationToken);
            return result.Success
                ? ApiCallResult<WeightItemDetailsProxy>.Ok(ProxyConverter.GetWeightItemDetailsProxy(result.Value))
                : ApiCallResult<WeightItemDetailsProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }
    }
}