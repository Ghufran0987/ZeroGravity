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
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Providers
{
    public class PersonalDataPageVmProvider : PageVmProviderBase, IPersonalDataPageVmProvider
    {
        private readonly IAlgorithmService _algorithmService;
        private readonly ILogger _logger;
        private readonly IUserDataService _userDataService;

        public PersonalDataPageVmProvider(ILoggerFactory loggerFactory, IUserDataService userDataService,
            IAlgorithmService algorithmService, ITokenService tokenService)
            : base(tokenService)
        {
            _userDataService = userDataService;
            _algorithmService = algorithmService;

            _logger = loggerFactory?.CreateLogger<PersonalDataPageVmProvider>() ??
                      new NullLogger<PersonalDataPageVmProvider>();
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

        public async Task<ApiCallResult<PersonalDataProxy>> CreatePersonalDataAsnyc(PersonalDataProxy personalDataProxy,
            CancellationToken cancellationToken)
        {
            var personalDataDto = ProxyConverter.GetPersonalDataDto(personalDataProxy);

            var apiCallResult = await _userDataService.CreatePersonalDataAsync(personalDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalDataProxy = ProxyConverter.GetPersonalDataProxy(apiCallResult.Value);

                DisplayConversionService.SetDisplayPrefences((DateTimeDisplayType)personalDataProxy.DateTimeDisplayType, (UnitDisplayType)personalDataProxy.UnitDisplayType);

                return ApiCallResult<PersonalDataProxy>.Ok(personalDataProxy);
            }

            return ApiCallResult<PersonalDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalDataProxy>> UpdatePersonalDataAsnyc(PersonalDataProxy personalDataProxy,
            CancellationToken cancellationToken)
        {
            var personalDataDto = ProxyConverter.GetPersonalDataDto(personalDataProxy);

            var apiCallResult = await _userDataService.UpdatePersonalDataAsync(personalDataDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalDataProxy = ProxyConverter.GetPersonalDataProxy(apiCallResult.Value);

                DisplayConversionService.SetDisplayPrefences((DateTimeDisplayType)personalDataProxy.DateTimeDisplayType, (UnitDisplayType)personalDataProxy.UnitDisplayType);

                return ApiCallResult<PersonalDataProxy>.Ok(personalDataProxy);
            }

            return ApiCallResult<PersonalDataProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> CreatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy,
            CancellationToken cancellationToken)
        {
            var personalGoalDto = ProxyConverter.GetPersonalGoalDto(personalGoalProxy);

            var apiCallResult = await _userDataService.CreatePersonalGoalAsync(personalGoalDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage);
        }

        public async Task<ApiCallResult<PersonalGoalProxy>> UpdatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy,
            CancellationToken cancellationToken)
        {
            var personalGoalDto = ProxyConverter.GetPersonalGoalDto(personalGoalProxy);

            var apiCallResult = await _userDataService.UpdatePersonalGoalAsync(personalGoalDto, cancellationToken);

            if (apiCallResult.Success)
            {
                personalGoalProxy = ProxyConverter.GetPersonalGoalProxy(apiCallResult.Value);

                return ApiCallResult<PersonalGoalProxy>.Ok(personalGoalProxy);
            }

            return ApiCallResult<PersonalGoalProxy>.Error(apiCallResult.ErrorMessage);
        }

        public PersonalGoalProxy CreateInitialTargets(int age, int gender, double weight, double bodyMassIndex,
            double bodyFatPercentage,
            double bloodGlucoseLevel, int fastingType)
        {
            var initialTargets = new PersonalGoalProxy();

            var bodyMassCategory = _algorithmService.GetBodyMassType(bodyMassIndex);

            var ageCategory = _algorithmService.GetAgeCategory(age);

            var bodyFatCategory = _algorithmService.GetBodyFatType(ageCategory, (GenderBiologicalType)gender, bodyFatPercentage);

            var bloodGlucoseType = _algorithmService.GetBloodGlucoseType(fastingType, bloodGlucoseLevel);


            var healthySnackPercentage = _algorithmService.SetInitialtargets(1, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.HealthySnackAmount =
                (int)_algorithmService.FoodAmountFromPercentage(healthySnackPercentage);

            var unhealthySnackPercentage = _algorithmService.SetInitialtargets(2, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.UnhealthySnackAmount =
                (int)_algorithmService.FoodAmountFromPercentage(unhealthySnackPercentage);

            var calorieDrinkPercentage = _algorithmService.SetInitialtargets(3, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.CalorieDrinkConsumption = _algorithmService.CalorieDrinkAmountFromPercentage(calorieDrinkPercentage);

            //multiply by 0.250 to get the actual consumption from cups
            initialTargets.WaterConsumption = _algorithmService.GetWaterAdvisory(weight) * 0.25;

            initialTargets.WaterConsumption = Math.Round(initialTargets.WaterConsumption, 2);

            var breakfastPercentage = _algorithmService.SetInitialtargets(4, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.BreakfastAmount = (int)_algorithmService.FoodAmountFromPercentage(breakfastPercentage);

            var lunchPercentage = _algorithmService.SetInitialtargets(5, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.LunchAmount = (int)_algorithmService.FoodAmountFromPercentage(lunchPercentage);

            var dinnerPercentage = _algorithmService.SetInitialtargets(6, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.DinnerAmount = (int)_algorithmService.FoodAmountFromPercentage(dinnerPercentage);


            var activityPercentage = _algorithmService.SetInitialtargets(8, (double)bodyFatCategory,
                (double)bodyMassCategory, (double)bloodGlucoseType);

            initialTargets.ActivityDuration = _algorithmService.ActivityAmountFromPercentage(activityPercentage);

            return initialTargets;
        }


        public double GetBmi(double height, double weight)
        {
            return _algorithmService.CalculateBmi(height, weight);
        }

        public double GetBfp(GenderBiologicalType sex, double waist, double hip, double neck, double height)
        {
            return _algorithmService.CalculateBodyFatPercentage(sex, waist, hip, neck, height);
        }
    }
}