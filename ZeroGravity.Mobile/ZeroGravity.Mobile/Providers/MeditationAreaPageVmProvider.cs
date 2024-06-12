using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class MeditationAreaPageVmProvider : PageVmProviderBase, IMeditationAreaPageVmProvider
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IApiService _api;
        private readonly IMeditationService _meditationService;

        public MeditationAreaPageVmProvider(ITokenService tokenService, ISecureStorageService secureStorageService, IApiService api, IMeditationService meditationService) : base(tokenService)
        {
            _secureStorageService = secureStorageService;
            _api = api;
            _meditationService = meditationService;
        }

        public async Task SaveTimeInLocalStorageAsync(TimeSpan t)
        {
            try
            {
                await _secureStorageService.SaveValue(SecureStorageKey.MeditationTime, t);
            }
            catch
            {
                //
            }
        }

        public async Task<TimeSpan> LoadTimeFromLocalStorageAsync()
        {
            try
            {
                return await _secureStorageService.LoadValue<TimeSpan>(SecureStorageKey.MeditationTime);
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        public async Task RemoveTimeFromLocalStorageAsync()
        {
            try
            {
                await _secureStorageService.Remove(SecureStorageKey.MeditationTime);
            }
            catch
            {
            }
        }

        public async Task<ApiCallResult<IEnumerable<StreamContentDto>>> GetAvailableStreamContentAsync(CancellationToken token = new CancellationToken())
        {
            var baseUrl = Common.ServerUrl;
            var type = (int)StreamContentType.Meditation;
            // var url = $"{baseUrl}/stream/{type}/all";
            var url = $"{baseUrl}/video/{type}/all";

            try
            {
                var list = await _api.GetAllJsonAsync<StreamContentDto>(url, token);

                var result = ApiCallResult<IEnumerable<StreamContentDto>>.Ok(list);

                return result;
            }
            catch (Exception e)
            {
                return ApiCallResult<IEnumerable<StreamContentDto>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<MeditationDataProxy>> SaveMeditationDataAsync(MeditationDataProxy meditationDataProxy, CancellationToken cancellationToken = new CancellationToken())
        {
            var meditationDataDto = ProxyConverter.GetMeditationDataDto(meditationDataProxy);

            var apiCallResult = await _meditationService.SaveMeditationDataAsync(meditationDataDto, cancellationToken);

            if (!apiCallResult.Success)
            {
                return ApiCallResult<MeditationDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
            }

            var meditationProxy = ProxyConverter.GetMeditationDataProxy(apiCallResult.Value);
            return ApiCallResult<MeditationDataProxy>.Ok(meditationProxy);
        }

        public async Task<ApiCallResult<TimeSpan>> GetMeditationDurationForDateAsync(DateTime date, CancellationToken cancellationToken)
        {
            var serviceResult = await _meditationService.GetAllMeditationDataForDateAsync(date, cancellationToken);
            if (!serviceResult.Success)
            {
                return ApiCallResult<TimeSpan>.Error(serviceResult.ErrorMessage, serviceResult.ErrorReason);
            }

            var meditationDataProxys = serviceResult.Value.Select(ProxyConverter.GetMeditationDataProxy);

            TimeSpan meditationDurationSum;
            foreach (var meditationDataProxy in meditationDataProxys)
            {
                meditationDurationSum += meditationDataProxy.Duration;
            }

            return ApiCallResult<TimeSpan>.Ok(meditationDurationSum);
        }
    }
}