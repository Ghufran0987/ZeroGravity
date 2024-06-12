using System;
using System.Collections.Generic;
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
using ZeroGravity.Shared.Models.Dto;
using System.Linq;

namespace ZeroGravity.Mobile.Providers
{
    public class WizardFinishSetupPageVmProvider : PageVmProviderBase, IWizardFinishSetupPageVmProvider
    {
        private readonly IUserDataService _userDataService;
        private readonly IAnalysisSummaryDataService _analysisSummaryDataService;
        private readonly ILogger _logger;

        public WizardFinishSetupPageVmProvider(IUserDataService userDataService,
            ITokenService tokenService, ILoggerFactory loggerFactory,
            IAnalysisSummaryDataService analysisSummaryDataService) : base(tokenService)
        {
            _userDataService = userDataService;
            _analysisSummaryDataService = analysisSummaryDataService;
            _logger = loggerFactory?.CreateLogger<WizardFinishSetupPageVmProvider>() ??
                      new NullLogger<WizardFinishSetupPageVmProvider>();
        }

        public async Task<ApiCallResult<PersonalDataProxy>> GetPersonalDataAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetPersonalDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var personalDataProxy = ProxyConverter.GetPersonalDataProxy(apiCallResult.Value);

                return ApiCallResult<PersonalDataProxy>.Ok(personalDataProxy);
            }

            return ApiCallResult<PersonalDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> GetPersonalGoalAsnyc(CancellationToken cancellationToken)
        {
            var apiCallResult = await _userDataService.GetPersonalGoalAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                var personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> CreatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken)
        {
            var personalGoalDto = ProxyConverter.GetPersonalGoalDto(personalGoalProxy);

            var apiCallResult = await _userDataService.CreatePersonalGoalAsync(personalGoalDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> UpdatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken)
        {
            var personalGoalDto = ProxyConverter.GetPersonalGoalDto(personalGoalProxy);

            var apiCallResult = await _userDataService.UpdatePersonalGoalAsync(personalGoalDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<bool>> UpdateWizardCompletionStatusAsync(UpdateWizardRequestDto updateDto, CancellationToken token)
        {
            var apiResult = await _userDataService.UpdateWizardStateAsync(updateDto, token);

            if (apiResult.Success)
            {
                return ApiCallResult<bool>.Ok(true);
            }

            return ApiCallResult<bool>.Error(apiResult.ErrorMessage);
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
                var waterProxy = new AnalysisItemProxy("\uf75c", AppResources.AnalysisPage_Items_Water, isHealthy, waterGoal, waterGoalMax, waterSum, waterSumMax, Convert.ToString(dto.PersonalGoalDto.WaterConsumption), true);
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
                    new AnalysisItemProxy("\uf7f6", AppResources.AnalysisPage_Items_Breakfast, isHealthy, breakfastGoal, breakfastGoalMax, breakfastSum, breakfastSumMax, dto.PersonalGoalDto.BreakfastAmount.ToString(), true);
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
                var lunchProxy = new AnalysisItemProxy("\uf81f", AppResources.AnalysisPage_Items_Lunch, isHealthy, lunchGoal, lunchGoalMax, lunchSum, lunchSumMax, dto.PersonalGoalDto.LunchAmount.ToString(), true);
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
                var dinnerProxy = new AnalysisItemProxy("\uf817", AppResources.AnalysisPage_Items_Dinner, isHealthy, dinnerGoal, dinnerGoalMax, dinnerSum, dinnerSumMax, dto.PersonalGoalDto.DinnerAmount.ToString(), true);
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
                    isHealthy, calorieDrinkAndAlcoholGoal, calorieDrinkAndAlcoholGoalMax, calorieDrinkAndAlcoholSum, calorieDrinkAndAlcoholSumMax, dto.PersonalGoalDto.CalorieDrinkConsumption.ToString(), true);
                calorieDrinkAndAlcoholProxy.ShowCalorieCupProgress = true;

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

                var healthySnackProxy = new AnalysisItemProxy("\uf5d1", AppResources.AnalysisPage_Items_HealthySnack, isHealthy, healthySnackGoal, healthySnackGoalMax, healthySnackSum, healthySnackSumMax, dto.PersonalGoalDto.HealthySnackAmount.ToString(), true);
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

                var unhealthySnackProxy = new AnalysisItemProxy("\uf564", AppResources.AnalysisPage_Items_UnhealthySnack, isHealthy, unhealthySnackGoal, unhealthySnackGoalMax, unhealthySnackSum, unhealthySnackSumMax, dto.PersonalGoalDto.UnhealthySnackAmount.ToString(), true);
                unhealthySnackProxy.ShowEllipseProgress = true;

                var activitySum = dto.ActivityDataDtos.Where(_ => _.ActivityType == ActivityType.Exercise)
                    .Sum(_ => _.Duration);
                var activityGoal = TimeSpan.FromMinutes(dto.PersonalGoalDto.ActivityDuration).TotalHours; // Convert from Minute to Hours

                double activityGoalMax = 16.0;// Maximum with Goals
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
                    new AnalysisItemProxy("\uf554", AppResources.AnalysisPage_Items_Activity, isHealthy, activityGoal, activityGoalMax, activitySum, activitySumMax, Convert.ToString(dto.PersonalGoalDto.ActivityDuration), true);
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
    }
}