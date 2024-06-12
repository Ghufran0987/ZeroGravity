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
    public class DayToDayPageVmProvider : PageVmProviderBase, IDayToDayPageVmProvider
    {
        private readonly IActivityDataService _activityDataService;
        private readonly ILogger _logger;

        public DayToDayPageVmProvider(ILoggerFactory loggerFactory, IActivityDataService activityDataService, ITokenService tokenService) : base(tokenService)
        {
            _activityDataService = activityDataService;
            _logger = loggerFactory?.CreateLogger<DayToDayPageVmProvider>() ??
                      new NullLogger<DayToDayPageVmProvider>();
        }

        public async Task<ApiCallResult<DayToDayActivityProxy>> GetDayToDayActivityAsnyc(int activityId, CancellationToken cancellationToken)
        {
            var apiCallResult = await _activityDataService.GetActivityDataAsync(activityId, cancellationToken);

            if (apiCallResult.Success)
            {
                var dayToDayActivityProxy = ProxyConverter.GetDayToDayActivityProxy(apiCallResult.Value);

                return ApiCallResult<DayToDayActivityProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<DayToDayActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<DayToDayActivityProxy>> GetDayToDayActivityByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _activityDataService.GetActivityDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var dayToDayActivityProxy = ProxyConverter.GetDayToDayActivityProxy(apiCallResult.Value);

                return ApiCallResult<DayToDayActivityProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<DayToDayActivityProxy >.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<DayToDayActivityProxy>> CreatDayToDayActivityAsnyc(DayToDayActivityProxy dayToDayActivityProxy, CancellationToken cancellationToken)
        {
            var activityDataDto = ProxyConverter.GetActivityDataDto(dayToDayActivityProxy);

            var apiCallResult = await _activityDataService.CreateActivityDataAsync(activityDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                dayToDayActivityProxy = ProxyConverter.GetDayToDayActivityProxy(apiCallResult.Value);

                return ApiCallResult<DayToDayActivityProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<DayToDayActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<DayToDayActivityProxy>> UpdateDayToDayActivityAsnyc(DayToDayActivityProxy dayToDayActivityProxy, CancellationToken cancellationToken)
        {
            var activityDataDto = ProxyConverter.GetActivityDataDto(dayToDayActivityProxy);

            var apiCallResult = await _activityDataService.UpdateActivityDataAsync(activityDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                dayToDayActivityProxy = ProxyConverter.GetDayToDayActivityProxy(apiCallResult.Value);

                return ApiCallResult<DayToDayActivityProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<DayToDayActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}