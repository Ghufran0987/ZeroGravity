using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class SugarBeatEatingSessionProvider : PageVmProviderBase, ISugarBeatEatingSessionProvider
    {
        private readonly ISugarBeatEatingSessionService _service;

        public SugarBeatEatingSessionProvider(ITokenService tokenService, ISugarBeatEatingSessionService service) : base(tokenService)
        {
            _service = service;
        }

        public async Task<ApiCallResult<SugarBeatEatingSessionProxy>> AddSugarBeatEatingSessionAsync(SugarBeatEatingSessionProxy proxy, CancellationToken cancellationToken)
        {

            var dto = ProxyConverter.GetSugarBeatEatingSessionDto(proxy);
            var result = await _service.AddAsync(dto, cancellationToken);

            var retResult = ProxyConverter.GetSugarBeatEatingSessionProxy(result.Value);

            return result?.Success == true
                ? ApiCallResult<SugarBeatEatingSessionProxy>.Ok(retResult)
                : ApiCallResult<SugarBeatEatingSessionProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<SugarBeatEatingSessionProxy>> GetSugarBeatEatingSessionAsync(int id, CancellationToken cancellationToken)
        {
            var apiCallResult = await _service.GetByIdAsync(id,cancellationToken);

            return apiCallResult?.Success == true
                ? ApiCallResult<SugarBeatEatingSessionProxy>.Ok(ProxyConverter.GetSugarBeatEatingSessionProxy(apiCallResult.Value))
                : ApiCallResult<SugarBeatEatingSessionProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<IEnumerable<SugarBeatEatingSessionProxy>>> GetSugarBeatEatingSessionsAsync(DateTime start, DateTime end, CancellationToken cancellationToken)
        {
            var apiCallResult = await _service.GetSugarBeatEatingSessionForPeriodAsync(start,end, cancellationToken);

            if (apiCallResult?.Success == true)
            {
                var proxies = new List<SugarBeatEatingSessionProxy>();
                if (apiCallResult.Value != null)
                {
                    foreach (var dto in apiCallResult.Value)
                    {
                        var fastingDataProxy = ProxyConverter.GetSugarBeatEatingSessionProxy(dto);
                        proxies.Add(fastingDataProxy);
                    }

                    return ApiCallResult<IEnumerable<SugarBeatEatingSessionProxy>>.Ok(proxies);
                }
                else
                {
                    return ApiCallResult<IEnumerable<SugarBeatEatingSessionProxy>>.Ok(proxies);
                }
            }

            return ApiCallResult<IEnumerable<SugarBeatEatingSessionProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<SugarBeatEatingSessionProxy>> UpdateSugarBeatEatingSessionAsync(SugarBeatEatingSessionProxy proxy, CancellationToken cancellationToken)
        {
            var dto = ProxyConverter.GetSugarBeatEatingSessionDto(proxy);
            var result = await _service.UpdateAsync(dto, cancellationToken);
            return result?.Success == true
                ? ApiCallResult<SugarBeatEatingSessionProxy>.Ok(ProxyConverter.GetSugarBeatEatingSessionProxy(result.Value))
                : ApiCallResult<SugarBeatEatingSessionProxy>.Error(result.ErrorMessage, result.ErrorReason);
        }

        public async Task<ApiCallResult<bool>> IsSenorWarmedUp(int sessionId, CancellationToken cancellationToken)
        {
            var result = await _service.GetIsSensorWarmedUp(sessionId, cancellationToken);
            return result?.Success == true
               ? ApiCallResult<bool>.Ok(result.Value)
               : ApiCallResult<bool>.Error(result.ErrorMessage, result.ErrorReason);
        }

    }
}
