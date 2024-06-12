using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class MonthlyReportPageVmProvider : PageVmProviderBase, IMonthlyReportPageVmProvider
    {
        private readonly IAnalysisSummaryDataService _analysisSummaryDataService;
        private readonly ILogger _logger;
        private IUserDataService _userDataService;
        private readonly IProgressSummaryDataService _progressSummaryDataService;

        public MonthlyReportPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService,
            IAnalysisSummaryDataService analysisSummaryDataService, IProgressSummaryDataService progressSummaryDataService,
             IUserDataService userDataService) : base(tokenService)
        {
            _analysisSummaryDataService = analysisSummaryDataService;
            _userDataService = userDataService;
            _progressSummaryDataService = progressSummaryDataService;
            _logger = loggerFactory?.CreateLogger<AnalysisPageVmProvider>() ??
                      new NullLogger<AnalysisPageVmProvider>();
        }

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDetailsAsync(CancellationToken cancellationToken)
        {
            var apiCallResult =
                await _userDataService.GetAccountDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<ProgressProxy>> GetProgressDataByDateAsync(DateTime fromDate, DateTime toDate,bool isMetascoreDeviceActive, CancellationToken cancellationToken)
        {
            try
            {
                ApiCallResult<List<ProgressSummaryDto>> result = null;

                result = await _progressSummaryDataService.GetAnalysisSummaryDataByPeriodAsync(fromDate, toDate, cancellationToken);

                if (result == null || result.Value == null)
                {
                    return ApiCallResult<ProgressProxy>.Error(result.ErrorMessage);
                }
                if ((!result.Success))
                {
                    return ApiCallResult<ProgressProxy>.Error(result.ErrorMessage);
                }

                var progressProxyModel = new ProgressProxy();
                var progressSummaryDtos = result.Value;

                foreach (var progressSummaryDto in progressSummaryDtos)
                {
                    switch (progressSummaryDto.Category)
                    {
                        case "Water":
                            Double actualWater = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualWater);
                            progressProxyModel.WaterProgress =
                                new LiquidProgress("\uf75c", AppResources.AnalysisPage_Items_Water, actualWater, progressSummaryDto.Goal);
                            break;

                        case "Drinks":
                            Double actualDrinks = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualDrinks);

                            progressProxyModel.CalorieProgress =
                            new LiquidProgress("\uf869", AppResources.AnalysisPage_Items_CalorieDrink, actualDrinks, progressSummaryDto.Goal);
                            break;

                        case "Breakfast":
                            Double actualbreakfast = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualbreakfast);
                            progressProxyModel.BreakFastProgress =
                            new FoodProgress("\uf7f6", AppResources.AnalysisPage_Items_Breakfast, (FoodAmountType)(int)actualbreakfast, (FoodAmountType)(int)progressSummaryDto.Goal);
                            break;

                        case "Dinner":
                            Double actualDinner = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualDinner);
                            progressProxyModel.DinnerProgress =
                             new FoodProgress("\uf817", AppResources.AnalysisPage_Items_Dinner, (FoodAmountType)(int)actualDinner, (FoodAmountType)(int)progressSummaryDto.Goal);

                            break;

                        case "Lunch":
                            Double actualLunch = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualLunch);
                            progressProxyModel.LunchProgress = new FoodProgress("\uf81f", AppResources.AnalysisPage_Items_Lunch, (FoodAmountType)(int)actualLunch, (FoodAmountType)(int)progressSummaryDto.Goal);
                            break;

                        case "HealthySnack":
                            Double actualHealthySnack = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualHealthySnack);

                            progressProxyModel.HealthySnacksProgress =
                                new FoodProgress("\uf5d1", AppResources.AnalysisPage_Items_HealthySnack, (FoodAmountType)(int)actualHealthySnack, (FoodAmountType)(int)progressSummaryDto.Goal);
                            break;

                        case "UnhealthySnack":
                            Double actualUnhealthySnack = 0.0;
                            double.TryParse(progressSummaryDto.Actual, out actualUnhealthySnack);

                            progressProxyModel.UnHealthySnacksProgress
                                = new FoodProgress("\uf564", AppResources.AnalysisPage_Items_UnhealthySnack, (FoodAmountType)(int)actualUnhealthySnack, (FoodAmountType)(int)progressSummaryDto.Goal);
                            break;

                        case "Activity":
                            TimeSpan actualActivity = TimeSpan.FromHours(0);
                            if (!string.IsNullOrEmpty(progressSummaryDto.Actual))
                            {
                                TimeSpan.TryParse(progressSummaryDto.Actual, out actualActivity);
                            }

                            TimeSpan goalActivity = TimeSpan.FromHours(progressSummaryDto.Goal);
                            progressProxyModel.ActivitiesProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                GetActivityStatus(actualActivity.TotalMinutes), GetActivityStatus(goalActivity.TotalMinutes), goalActivity.TotalMinutes, actualActivity.TotalMinutes);

                            progressProxyModel.ActivitiesProgress.TargetFood = Convert.ToInt32(goalActivity.TotalMinutes).ToString();
                            progressProxyModel.ActivitiesProgress.ActualFood = Convert.ToInt32(actualActivity.TotalMinutes).ToString();
                            break;

                        case "Fasting":
                            TimeSpan actualFasting = TimeSpan.FromHours(0);
                            if (!string.IsNullOrEmpty(progressSummaryDto.Actual))
                            {
                                TimeSpan.TryParse(progressSummaryDto.Actual, out actualActivity);
                            }
                            TimeSpan goalFasting = TimeSpan.FromHours(progressSummaryDto.Goal);
                            progressProxyModel.FastingProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                GetFastingStatus(actualFasting.TotalHours), GetFastingStatus(goalFasting.TotalHours));
                            break;

                        case "Meditation":
                            TimeSpan actualMeditation = TimeSpan.FromMinutes(0);
                            if (!string.IsNullOrEmpty(progressSummaryDto.Actual))
                            {
                                TimeSpan.TryParse(progressSummaryDto.Actual, out actualActivity);
                            }
                            TimeSpan goalMeditation = TimeSpan.FromMinutes(progressSummaryDto.Goal);
                            progressProxyModel.MeditationProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                GetmeditationSatatus(actualMeditation.TotalMinutes), GetmeditationSatatus(goalMeditation.TotalMinutes), goalMeditation.TotalMinutes, actualMeditation.TotalMinutes);

                            progressProxyModel.MeditationProgress.TargetFood = Convert.ToInt32(goalMeditation.TotalMinutes).ToString();
                            progressProxyModel.MeditationProgress.ActualFood = Convert.ToInt32(actualMeditation.TotalMinutes).ToString();
                            break;

                        case "Metascore":
                            if(isMetascoreDeviceActive)
                            {
                                double actualMetaScore = 0.0;
                                double.TryParse(progressSummaryDto.Actual, out actualMetaScore);
                                progressProxyModel.MetaScore = new MetabolicProgress((int)actualMetaScore);
                            }
                            else
                            {
                                progressProxyModel.MetaScore = new MetabolicProgress(-1);
                            }
                            break;

                        default:
                            break;
                    }
                }

                progressProxyModel.GetTotalScore(isMetascoreDeviceActive);

                progressProxyModel.GetMessages();
                return ApiCallResult<ProgressProxy>.Ok(progressProxyModel);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                return ApiCallResult<ProgressProxy>.Error(e.Message);
            }
        }

        private FoodAmountType GetActivityStatus(double ActivityAmount)
        {
            var activityAmountType = FoodAmountType.None;
            if (ActivityAmount >= 40)
            {
                activityAmountType = FoodAmountType.VeryHeavy;
            }
            else if (ActivityAmount >= 35 && ActivityAmount < 40)
            {
                activityAmountType = FoodAmountType.Heavy;
            }
            else if (ActivityAmount >= 30 && ActivityAmount < 35)
            {
                activityAmountType = FoodAmountType.Medium;
            }
            else if (ActivityAmount >= 25 && ActivityAmount < 30)
            {
                activityAmountType = FoodAmountType.Light;
            }
            else if (ActivityAmount >= 20 && ActivityAmount < 25)
            {
                activityAmountType = FoodAmountType.VeryLight;
            }
            else if (ActivityAmount < 0)
            {
                activityAmountType = FoodAmountType.None;
            }
            return activityAmountType;
        }

        private FoodAmountType GetFastingStatus(double fastingAmount)
        {
            var activityAmountType = FoodAmountType.None;
            if (fastingAmount >= 24)
            {
                activityAmountType = FoodAmountType.VeryHeavy;
            }
            else if (fastingAmount >= 20 && fastingAmount < 24)
            {
                activityAmountType = FoodAmountType.Heavy;
            }
            else if (fastingAmount >= 16 && fastingAmount < 20)
            {
                activityAmountType = FoodAmountType.Medium;
            }
            else if (fastingAmount >= 12 && fastingAmount < 16)
            {
                activityAmountType = FoodAmountType.Light;
            }
            else if (fastingAmount >= 8 && fastingAmount < 12)
            {
                activityAmountType = FoodAmountType.VeryLight;
            }
            else if (fastingAmount < 8)
            {
                activityAmountType = FoodAmountType.None;
            }
            return activityAmountType;
        }

        private FoodAmountType GetmeditationSatatus(double meditationAmount)
        {
            var meditationAmountType = FoodAmountType.None;
            if (meditationAmount >= 30)
            {
                meditationAmountType = FoodAmountType.VeryHeavy;
            }
            else if (meditationAmount >= 25 && meditationAmount < 30)
            {
                meditationAmountType = FoodAmountType.Heavy;
            }
            else if (meditationAmount >= 20 && meditationAmount < 25)
            {
                meditationAmountType = FoodAmountType.Medium;
            }
            else if (meditationAmount >= 15 && meditationAmount < 20)
            {
                meditationAmountType = FoodAmountType.Light;
            }
            else if (meditationAmount >= 10 && meditationAmount < 15)
            {
                meditationAmountType = FoodAmountType.VeryLight;
            }
            else if (meditationAmount < 10)
            {
                meditationAmountType = FoodAmountType.None;
            }
            return meditationAmountType;
        }
    }
}