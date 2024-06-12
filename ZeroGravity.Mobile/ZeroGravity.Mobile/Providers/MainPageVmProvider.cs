using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class MainPageVmProvider : PageVmProviderBase, IMainPageVmProvider
    {
        private readonly IBadgeInformationService _badgeInformationService;
        private readonly ITrackedHistoryService _trackedHistoryService;
        private readonly ILogger _logger;
        private IBleService _bleService;

        private ISecureStorageService _secureStorageService;
        public MainPageVmProvider(ILoggerFactory loggerFactory, ILoginService loginService, ITokenService tokenService,
            IBadgeInformationService badgeInformationService, ITrackedHistoryService trackedHistoryService,
            IBleService bleService, ISecureStorageService secureStorageService)
            : base(tokenService)
        {
            _bleService = bleService;
            _badgeInformationService = badgeInformationService;
            _trackedHistoryService = trackedHistoryService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<MainPageVmProvider>() ?? new NullLogger<MainPageVmProvider>();
        }


        public async Task<SugarBeatAccessDetailsProxy> GetSugarBeatAccessDetailsAsync()
        {
            try
            {
                var model = await _secureStorageService.LoadObject<SugarBeatAccessDetails>(SecureStorageKey.SugarBeatAccessDetails) ?? new SugarBeatAccessDetails { MacAddress = string.Empty, Password = string.Empty };

                return await Task.FromResult(new SugarBeatAccessDetailsProxy
                {
                    Address = model.MacAddress,
                    Password = model.Password
                });
            }
            catch
            {
                return await Task.FromResult(new SugarBeatAccessDetailsProxy
                {
                    Address = string.Empty,
                    Password = string.Empty
                });
            }
        }

        public async Task<ApiCallResult<MyDayBadgeInformationProxy>> GetMyDayBadgeInformationAsnyc(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _badgeInformationService.GetMyDayBadgeInformationByDateAsync(targetDateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var mealsBadgeInformationProxy = ProxyConverter.GetMyDayBadgeInformationProxy(apiCallResult.Value);

                return ApiCallResult<MyDayBadgeInformationProxy>.Ok(mealsBadgeInformationProxy);
            }

            return ApiCallResult<MyDayBadgeInformationProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<List<TrackedHistoryProxy>>> GetTrackedHistoryAsnyc(DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _trackedHistoryService.GetTrackedHistoryByDateAsync(targetDateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                List<TrackedHistoryProxy> trackedHistoryProxies = new List<TrackedHistoryProxy>();

                foreach (var trackedHistoryDto in apiCallResult.Value)
                {
                    var trackedHistoryProxy = ProxyConverter.GetTrackedHistoryProxy(trackedHistoryDto);

                    trackedHistoryProxies.Add(trackedHistoryProxy);
                }

                return ApiCallResult<List<TrackedHistoryProxy>>.Ok(trackedHistoryProxies);
            }

            return ApiCallResult<List<TrackedHistoryProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public bool CheckSugarbeatConnected()
        {
            return _bleService.CheckSugarbeatConnected();
        }
    }
}