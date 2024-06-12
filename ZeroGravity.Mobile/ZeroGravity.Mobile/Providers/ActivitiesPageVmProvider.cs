using System;
using System.Collections.Generic;
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
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Providers
{
    public class ActivitiesPageVmProvider : PageVmProviderBase, IActivitiesPageVmProvider
    {
        private readonly ILogger _logger;
        private readonly IActivityDataService _activityDataService;
        private readonly IFitbitIntegrationService _fitbitIntegrationService;
        private readonly IIntegrationDataService _integrationDataService;

        public ActivitiesPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService,
            IActivityDataService activityDataService, IFitbitIntegrationService fitbitIntegrationService, IIntegrationDataService integrationDataService) : base(tokenService)
        {
            _activityDataService = activityDataService;
            _fitbitIntegrationService = fitbitIntegrationService;
            _integrationDataService = integrationDataService;

            _logger = loggerFactory?.CreateLogger<ActivitiesPageVmProvider>() ??
                      new NullLogger<ActivitiesPageVmProvider>();
        }

        public async Task<ApiCallResult<ExerciseActivityProxy>> SaveActivityAsync(ExerciseActivityProxy proxy,
            CancellationToken cancellationToken)
        {
            var activityDataDto = ProxyConverter.GetActivityDataDto(proxy);
            var apiCallResult = await _activityDataService.CreateActivityDataAsync(activityDataDto, cancellationToken);

            if (!apiCallResult.Success)
            {
                return ApiCallResult<ExerciseActivityProxy>.Error(apiCallResult.ErrorMessage,
                    apiCallResult.ErrorReason);
            }

            var exerciseActivityProxy = ProxyConverter.GetExerciseActivityProxy(apiCallResult.Value);
            return ApiCallResult<ExerciseActivityProxy>.Ok(exerciseActivityProxy);
        }

        public async Task<ApiCallResult<double>> GetActivitySummary(DateTime dateTime, CancellationToken cancellationToken)
        {
            var serviceResult = await _activityDataService.GetActivitiesByDateAsync(dateTime, cancellationToken);
            if (!serviceResult.Success)
            {
                return ApiCallResult<double>.Error(serviceResult.ErrorMessage, serviceResult.ErrorReason);
            }

            var summary = serviceResult.Value
                .Where(_ => _.ActivityType == ActivityType.Exercise)
                .Select(ProxyConverter.GetExerciseActivityProxy)
                .Sum(_ => _.Duration);
            return ApiCallResult<double>.Ok(summary);
        }

        public async Task<ApiCallResult<List<IntegrationDataProxy>>> GetLinkedIntegrationListAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _integrationDataService.GetLinkedIntegrationListAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                List<IntegrationDataProxy> integrationDataProxies = new List<IntegrationDataProxy>();

                foreach (var integrationDataDto in apiCallResult.Value)
                {
                    var integrationDataProxy = ProxyConverter.GetIntegrationDataProxy(integrationDataDto);

                    integrationDataProxies.Add(integrationDataProxy);
                }

                
                return ApiCallResult<List<IntegrationDataProxy>>.Ok(integrationDataProxies);
            }

            return ApiCallResult<List<IntegrationDataProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<List<SyncActivityProxy>>> GetActivitySyncDataAsnyc(IntegrationDataProxy integrationDataProxy, DateTime targetDateTime,
            CancellationToken cancellationToken)
        {
            if (integrationDataProxy.Name.Equals(IntegrationNameConstants.Fitbit))
            {
                var apiCallResult = await _fitbitIntegrationService.GetFitbitActivitiesAsync(integrationDataProxy.Id, targetDateTime, cancellationToken);

                if (apiCallResult.Success)
                {
                    apiCallResult.Value.TargetDate = targetDateTime;
                    apiCallResult.Value.IntegrationId = integrationDataProxy.Id;

                    var syncActivities = ProxyConverter.GetFitbitActivityProxies(apiCallResult.Value);

                    return ApiCallResult<List<SyncActivityProxy>>.Ok(syncActivities);
                }

                
                return ApiCallResult<List<SyncActivityProxy>>.Error(apiCallResult.ErrorMessage);
            }


            return ApiCallResult<List<SyncActivityProxy>>.Error("");
        }
    }
}