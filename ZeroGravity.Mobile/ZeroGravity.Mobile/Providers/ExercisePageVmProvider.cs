using System;
using System.Collections.ObjectModel;
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
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Providers
{
    public class ExercisePageVmProvider : PageVmProviderBase, IExercisePageVmProvider
    {
        private readonly IActivityDataService _activityDataService;
        private readonly ILogger _logger;

        public ExercisePageVmProvider(ILoggerFactory loggerFactory, IActivityDataService activityDataService,
            ITokenService tokenService) : base(tokenService)
        {
            _activityDataService = activityDataService;
            _logger = loggerFactory?.CreateLogger<ExercisePageVmProvider>() ??
                      new NullLogger<ExercisePageVmProvider>();
        }

        public async Task<ApiCallResult<ExerciseActivityProxy>> GetExerciseActivityAsnyc(int activityId, CancellationToken cancellationToken)
        {
            var apiCallResult = await _activityDataService.GetActivityDataAsync(activityId, cancellationToken);

            if (apiCallResult.Success)
            {
                var exerciseActivityProxy = ProxyConverter.GetExerciseActivityProxy(apiCallResult.Value);

                return ApiCallResult<ExerciseActivityProxy>.Ok(exerciseActivityProxy);
            }

            return ApiCallResult<ExerciseActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<ExerciseActivityProxy>> GetExerciseActivityByDateAsnyc(DateTime dateTime, CancellationToken cancellationToken)
        {
            var apiCallResult = await _activityDataService.GetActivityDataByDateAsync(dateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var dayToDayActivityProxy = ProxyConverter.GetExerciseActivityProxy(apiCallResult.Value);

                return ApiCallResult<ExerciseActivityProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<ExerciseActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<ExerciseActivityProxy>> CreateExerciseActivityAsnyc(ExerciseActivityProxy exerciseActivityProxy, CancellationToken cancellationToken)
        {
            var activityDataDto = ProxyConverter.GetActivityDataDto(exerciseActivityProxy);

            var apiCallResult = await _activityDataService.CreateActivityDataAsync(activityDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                exerciseActivityProxy = ProxyConverter.GetExerciseActivityProxy(apiCallResult.Value);

                return ApiCallResult<ExerciseActivityProxy>.Ok(exerciseActivityProxy);
            }

            return ApiCallResult<ExerciseActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<ExerciseActivityProxy>> UpdateExerciseActivitysnyc(ExerciseActivityProxy exerciseActivityProxy, CancellationToken cancellationToken)
        {
            var activityDataDto = ProxyConverter.GetActivityDataDto(exerciseActivityProxy);

            var apiCallResult = await _activityDataService.UpdateActivityDataAsync(activityDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                exerciseActivityProxy = ProxyConverter.GetExerciseActivityProxy(apiCallResult.Value);

                return ApiCallResult<ExerciseActivityProxy>.Ok(exerciseActivityProxy);
            }

            return ApiCallResult<ExerciseActivityProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public ObservableCollection<ComboBoxItem> GetExerciseItems()
        {
            ObservableCollection<ComboBoxItem> exerciseItems = new ObservableCollection<ComboBoxItem>();

            exerciseItems.Add(new ComboBoxItem {Id = (int) ExerciseType.Ride, Text = AppResources.ExerciseItems_Ride});
            exerciseItems.Add(new ComboBoxItem {Id = (int) ExerciseType.Run, Text = AppResources.ExerciseItems_Run});
            exerciseItems.Add(new ComboBoxItem {Id = (int) ExerciseType.Swim, Text = AppResources.ExerciseItems_Swim});
            exerciseItems.Add(new ComboBoxItem {Id = (int) ExerciseType.Hike, Text = AppResources.ExerciseItems_Hike});
            exerciseItems.Add(new ComboBoxItem {Id = (int) ExerciseType.Workout, Text = AppResources.ExerciseItems_Workout});
            exerciseItems.Add(new ComboBoxItem {Id = (int) ExerciseType.Yoga, Text = AppResources.ExerciseItems_Yoga});

            return exerciseItems;
        }
    }
}