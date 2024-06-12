using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services.Communication;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class AnalysisPageVmProvider : PageVmProviderBase, IAnalysisPageVmProvider
    {
        private readonly IAnalysisSummaryDataService _analysisSummaryDataService;
        private readonly ILogger _logger;

        private readonly IProgressSummaryDataService _progressSummaryDataService;

        public AnalysisPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService,
            IAnalysisSummaryDataService analysisSummaryDataService, IProgressSummaryDataService progressSummaryDataService) : base(tokenService)
        {
            _analysisSummaryDataService = analysisSummaryDataService;
            _progressSummaryDataService = progressSummaryDataService;
            _logger = loggerFactory?.CreateLogger<AnalysisPageVmProvider>() ??
                      new NullLogger<AnalysisPageVmProvider>();
        }

        public async Task<ApiCallResult<List<AnalysisItemProxy>>> GetAnalysisSummaryDataByDateAsync(
            DateTime dateTime, CancellationToken cancellationToken)
        {
            try
            {
                var result =
                    await _analysisSummaryDataService.GetAnalysisSummaryDataByDateAsync(dateTime, cancellationToken);
                if (!result.Success)
                {
                    return ApiCallResult<List<AnalysisItemProxy>>.Error(result.ErrorMessage);
                }

                var items = new List<AnalysisItemProxy>();
                var dto = result.Value;
                bool isHealthy = false;

                // Unterschiedliche Einheiten bei Goals und LiquidType.Water in der DB, Wieso??????
                var waterSum = dto.LiquidIntakeDtos.Where(_ => _.LiquidType == LiquidType.Water).Sum(_ => _.Amount) /
                               1000;
                var waterGoal = dto.PersonalGoalDto.WaterConsumption;
                double waterGoalMax = 5.0;// Maximum bei Zielen
                double waterSumMax;
                isHealthy = waterGoal <= waterSum;
                if (waterSum < waterGoalMax)
                {
                    waterSumMax = waterGoalMax;
                }
                else
                {
                    waterGoalMax = 5.0 * 1.1;
                    waterSumMax = 5.0 * 1.1;
                    waterSum = 5.0 * 1.1;
                }
                var waterProxy = new AnalysisItemProxy("\uf75c", AppResources.AnalysisPage_Items_Water, isHealthy, waterGoal, waterGoalMax, waterSum, waterSumMax, Convert.ToString(dto.PersonalGoalDto.WaterConsumption), false);
                waterProxy.ShowWaterCupProgress = true;

                var breakfasts = dto.MealDataDtos.Where(_ =>
                    _.MealSlotType == MealSlotType.Breakfast && _.Amount != FoodAmountType.Undefined);
                double breakfastSum = breakfasts.Select(_ => (int)_.Amount).Sum();
                var breakfastGoal = (int)dto.PersonalGoalDto.BreakfastAmount;

                double breakfastGoalMax = (int)FoodAmountType.VeryHeavy;// Maximum bei Zielen
                double breakfastSumMax;
                isHealthy = breakfastGoal >= breakfastSum;
                if (breakfastSum < breakfastGoalMax)
                {
                    breakfastSumMax = (int)FoodAmountType.VeryHeavy;
                }
                else
                {
                    breakfastGoalMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    breakfastSumMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    breakfastSum = (int)FoodAmountType.VeryHeavy * 1.1;
                }
                var breakfastProxy =
                    new AnalysisItemProxy("\uf7f6", AppResources.AnalysisPage_Items_Breakfast, isHealthy, breakfastGoal, breakfastGoalMax, breakfastSum, breakfastSumMax, dto.PersonalGoalDto.BreakfastAmount.ToString(), false);
                breakfastProxy.ShowEllipseProgress = true;

                var lunches = dto.MealDataDtos.Where(_ =>
                    _.MealSlotType == MealSlotType.Lunch && _.Amount != FoodAmountType.Undefined);
                double lunchSum = lunches.Select(_ => (int)_.Amount).Sum();
                var lunchGoal = (int)dto.PersonalGoalDto.LunchAmount;

                double lunchGoalMax = (int)FoodAmountType.VeryHeavy;// Maximum bei Zielen
                double lunchSumMax;
                isHealthy = lunchGoal >= lunchSum;
                if (lunchSum < lunchGoalMax)
                {
                    lunchSumMax = (int)FoodAmountType.VeryHeavy;
                }
                else
                {
                    lunchGoalMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    lunchSumMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    lunchSum = (int)FoodAmountType.VeryHeavy * 1.1;
                }
                var lunchProxy = new AnalysisItemProxy("\uf81f", AppResources.AnalysisPage_Items_Lunch, isHealthy, lunchGoal, lunchGoalMax, lunchSum, lunchSumMax, dto.PersonalGoalDto.LunchAmount.ToString(), false);
                lunchProxy.ShowEllipseProgress = true;

                var dinners = dto.MealDataDtos.Where(_ =>
                    _.MealSlotType == MealSlotType.Dinner && _.Amount != FoodAmountType.Undefined);
                double dinnerSum = dinners.Select(_ => (int)_.Amount).Sum();
                var dinnerGoal = (int)dto.PersonalGoalDto.DinnerAmount;

                double dinnerGoalMax = (int)FoodAmountType.VeryHeavy;// Maximum bei Zielen
                double dinnerSumMax;
                isHealthy = dinnerGoal >= dinnerSum;
                if (dinnerSum < dinnerGoalMax)
                {
                    dinnerSumMax = (int)FoodAmountType.VeryHeavy;
                }
                else
                {
                    dinnerGoalMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    dinnerSumMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    dinnerSum = (int)FoodAmountType.VeryHeavy * 1.1;
                }
                var dinnerProxy = new AnalysisItemProxy("\uf817", AppResources.AnalysisPage_Items_Dinner, isHealthy, dinnerGoal, dinnerGoalMax, dinnerSum, dinnerSumMax, dto.PersonalGoalDto.DinnerAmount.ToString(), false);
                dinnerProxy.ShowEllipseProgress = true;

                // Unterschiedliche Einheiten bei Goals und LiquidType.CalorieDrinkAndAlcohol in der DB, Wieso??????
                var calorieDrinkAndAlcoholSum = dto.LiquidIntakeDtos
                                                 .Where(_ => _.LiquidType == LiquidType.CalorieDrinkAndAlcohol)
                                                 .Sum(_ => _.Amount) /
                                             1000;
                var calorieDrinkAndAlcoholGoal = dto.PersonalGoalDto.CalorieDrinkConsumption;

                double calorieDrinkAndAlcoholGoalMax = 2.0;// Maximum bei Zielen
                double calorieDrinkAndAlcoholSumMax;
                isHealthy = calorieDrinkAndAlcoholGoal >= calorieDrinkAndAlcoholSum;
                if (calorieDrinkAndAlcoholSum < calorieDrinkAndAlcoholGoalMax)
                {
                    calorieDrinkAndAlcoholSumMax = calorieDrinkAndAlcoholGoalMax;
                }
                else
                {
                    calorieDrinkAndAlcoholGoalMax = 2.0 * 1.1;
                    calorieDrinkAndAlcoholSumMax = 2.0 * 1.1;
                    calorieDrinkAndAlcoholSum = 2.0 * 1.1;
                }

                var calorieDrinkAndAlcoholProxy = new AnalysisItemProxy("\uf869", AppResources.AnalysisPage_Items_CalorieDrink,
                    isHealthy, calorieDrinkAndAlcoholGoal, calorieDrinkAndAlcoholGoalMax, calorieDrinkAndAlcoholSum, calorieDrinkAndAlcoholSumMax, dto.PersonalGoalDto.CalorieDrinkConsumption.ToString(), false);
                calorieDrinkAndAlcoholProxy.ShowWaterCupProgress = true;

                var healthySnacks = dto.MealDataDtos.Where(_ =>
                    _.MealSlotType == MealSlotType.HealthySnack && _.Amount != FoodAmountType.Undefined);
                double healthySnackSum = healthySnacks.Select(_ => (int)_.Amount).Sum();
                var healthySnackGoal = (int)dto.PersonalGoalDto.HealthySnackAmount;

                double healthySnackGoalMax = (int)FoodAmountType.VeryHeavy;// Maximum bei Zielen
                double healthySnackSumMax;
                isHealthy = healthySnackGoal <= healthySnackSum;
                if (healthySnackSum < healthySnackGoalMax)
                {
                    healthySnackSumMax = (int)FoodAmountType.VeryHeavy;
                }
                else
                {
                    healthySnackGoalMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    healthySnackSumMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    healthySnackSum = (int)FoodAmountType.VeryHeavy * 1.1;
                }

                var healthySnackProxy = new AnalysisItemProxy("\uf5d1", AppResources.AnalysisPage_Items_HealthySnack, isHealthy, healthySnackGoal, healthySnackGoalMax, healthySnackSum, healthySnackSumMax, dto.PersonalGoalDto.HealthySnackAmount.ToString(), false);
                healthySnackProxy.ShowEllipseProgress = true;

                var unhealthySnacks = dto.MealDataDtos.Where(_ =>
                    _.MealSlotType == MealSlotType.UnhealthySnack && _.Amount != FoodAmountType.Undefined);
                double unhealthySnackSum = unhealthySnacks.Select(_ => (int)_.Amount).Sum();
                var unhealthySnackGoal = (int)dto.PersonalGoalDto.UnhealthySnackAmount;

                double unhealthySnackGoalMax = (int)FoodAmountType.VeryHeavy;// Maximum bei Zielen
                double unhealthySnackSumMax;
                isHealthy = unhealthySnackGoal >= unhealthySnackSum;
                if (unhealthySnackSum < unhealthySnackGoalMax)
                {
                    unhealthySnackSumMax = (int)FoodAmountType.VeryHeavy;
                }
                else
                {
                    unhealthySnackGoalMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    unhealthySnackSumMax = (int)FoodAmountType.VeryHeavy * 1.1;
                    unhealthySnackSum = (int)FoodAmountType.VeryHeavy * 1.1;
                }

                var unhealthySnackProxy = new AnalysisItemProxy("\uf564", AppResources.AnalysisPage_Items_UnhealthySnack, isHealthy, unhealthySnackGoal, unhealthySnackGoalMax, unhealthySnackSum, unhealthySnackSumMax, dto.PersonalGoalDto.UnhealthySnackAmount.ToString(), false);
                unhealthySnackProxy.ShowEllipseProgress = true;

                var activitySum = dto.ActivityDataDtos.Where(_ => _.ActivityType == ActivityType.Exercise)
                    .Sum(_ => _.Duration);

                var activityGoal = TimeSpan.FromMinutes(dto.PersonalGoalDto.ActivityDuration).TotalHours; // Convert from Minute to Hours

                double activityGoalMax = 16.0;// Maximum with goals
                double activitySumMax;
                isHealthy = activityGoal <= activitySum;
                if (activitySum < activityGoalMax)
                {
                    activitySumMax = activityGoalMax;
                }
                else
                {
                    activityGoalMax = 16.0 * 1.1;
                    activitySumMax = 16.0 * 1.1;
                    activitySum = 16.0 * 1.1;
                }

                var activityProxy =
                    new AnalysisItemProxy("\uf554", AppResources.AnalysisPage_Items_Activity, isHealthy, activityGoal, activityGoalMax, activitySum, activitySumMax, Convert.ToString(dto.PersonalGoalDto.ActivityDuration), false);
                activityProxy.ShowCircularProgress = true;

                items.Add(waterProxy);
                items.Add(breakfastProxy);
                items.Add(lunchProxy);
                items.Add(dinnerProxy);
                items.Add(calorieDrinkAndAlcoholProxy);
                items.Add(healthySnackProxy);
                items.Add(unhealthySnackProxy);
                items.Add(activityProxy);

                return ApiCallResult<List<AnalysisItemProxy>>.Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                return ApiCallResult<List<AnalysisItemProxy>>.Error(e.Message);
            }
        }

        public async Task<ApiCallResult<ProgressProxy>> GetProgressDataByDateAsync(DateTime dateTime, CancellationToken cancellationToken, string type)
        {
            try
            {
                ApiCallResult<List<ProgressSummaryDto>> result = null;
                if (type == "Today")
                {
                    result = await _progressSummaryDataService.GetAnalysisSummaryDataByDayAsync(dateTime, cancellationToken);
                }
                else if (type == "Last7Day")
                {
                    result = await _progressSummaryDataService.GetAnalysisSummaryDataByPeriodAsync(dateTime.AddDays(-7), dateTime, cancellationToken);
                }
                else if (type == "Last30Day")
                {
                    result = await _progressSummaryDataService.GetAnalysisSummaryDataByPeriodAsync(dateTime.AddDays(-30), dateTime, cancellationToken);
                }

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
                                GetActivityStatus(actualActivity.TotalMinutes), GetActivityStatus(goalActivity.TotalMinutes),0,0,true);
                            break;
                        case "Fasting":
                            TimeSpan actualFasting = TimeSpan.FromHours(0);
                            if (!string.IsNullOrEmpty(progressSummaryDto.Actual))
                            {
                                TimeSpan.TryParse(progressSummaryDto.Actual, out actualActivity);
                            }
                            TimeSpan goalFasting = TimeSpan.FromHours(progressSummaryDto.Goal);
                            progressProxyModel.FastingProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                GetFastingSatatus(actualFasting.TotalHours), GetFastingSatatus(goalFasting.TotalHours));
                            if (goalFasting.TotalHours == 0)
                            {
                                progressProxyModel.FastingProgress.IsFastingProgressVisible = false;
                            }
                            else
                            {
                                progressProxyModel.FastingProgress.IsFastingProgressVisible = true;
                            }
                            break;
                        case "Meditation":
                            TimeSpan actualMeditation = TimeSpan.FromMinutes(0);
                            if (!string.IsNullOrEmpty(progressSummaryDto.Actual))
                            {
                                TimeSpan.TryParse(progressSummaryDto.Actual, out actualActivity);
                            }
                            TimeSpan goalMeditation = TimeSpan.FromMinutes(progressSummaryDto.Goal);
                            progressProxyModel.MeditationProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                GetmeditationSatatus(actualMeditation.TotalMinutes), GetmeditationSatatus(goalMeditation.TotalMinutes));
                            break;
                        default:
                            break;
                    }
                }

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

        private FoodAmountType GetFastingSatatus(double fastingAmount)
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