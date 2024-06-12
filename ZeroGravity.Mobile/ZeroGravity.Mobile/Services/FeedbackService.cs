using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xamarin.Forms;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        private readonly ISecureStorageService _secureStorageService;

        public FeedbackService(ILoggerFactory loggerFactory, IApiService apiService,
            ISecureStorageService secureStorageService)
        {
            _apiService = apiService;
            _secureStorageService = secureStorageService;
            _logger = loggerFactory?.CreateLogger<FeedbackService>() ?? new NullLogger<FeedbackService>();
        }


        public async Task<ApiCallResult<FeedbackSummaryDto>> GetFeedbackSummaryDtoByDateAsync(DateTime targetDateTime,
            CancellationToken cancellationToken)
        {
            var accountId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var baseUrl = Common.ServerUrl;

            var api = $"/feedback/{accountId}/{DateTimeHelper.ToUniversalControllerDate(targetDateTime)}";
            var url = baseUrl + api;

            try
            {
                var feedbackSummaryDto =
                    await _apiService.GetSingleJsonAsync<FeedbackSummaryDto>(url, cancellationToken);

                return ApiCallResult<FeedbackSummaryDto>.Ok(feedbackSummaryDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetFeedbackSummaryDtoByDateAsync.");
                    return ApiCallResult<FeedbackSummaryDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<FeedbackSummaryDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<FeedbackSummaryDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<FeedbackSummaryDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public FeedbackDataProxy SetFeedbackState(FeedbackDataProxy feedbackDataProxy)
        {
            switch (feedbackDataProxy.FeedbackType)
            {
                case FeedbackType.Breakfast:
                    feedbackDataProxy.FeedbackState = GetMealFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    //Round actualvalue after calculation to ensure correct display
                    feedbackDataProxy.ActualValue = Math.Round(feedbackDataProxy.ActualValue, 0);
                    feedbackDataProxy.Recommendation = GetMealRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image =
                        GetBreakfastImageRessource((FoodAmountType) feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_BreakfastTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue , feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.Lunch:
                    feedbackDataProxy.FeedbackState = GetMealFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    //Round actualvalue after calculation to ensure correct display
                    feedbackDataProxy.ActualValue = Math.Round(feedbackDataProxy.ActualValue, 0);
                    feedbackDataProxy.Recommendation = GetMealRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image = GetLunchImageRessource((FoodAmountType) feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_LunchTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.Dinner:
                    feedbackDataProxy.FeedbackState = GetMealFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    //Round actualvalue after calculation to ensure correct display
                    feedbackDataProxy.ActualValue = Math.Round(feedbackDataProxy.ActualValue, 0);
                    feedbackDataProxy.Recommendation = GetMealRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image = GetDinnerImageRessource((FoodAmountType) feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_DinnerTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.HealthySnack:
                    feedbackDataProxy.FeedbackState = GetHealthySnackFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    //Round actualvalue after calculation to ensure correct display
                    feedbackDataProxy.ActualValue = Math.Round(feedbackDataProxy.ActualValue, 0);
                    feedbackDataProxy.Recommendation = GetHealthySnacksRecommendation(feedbackDataProxy.FeedbackState,
                        feedbackDataProxy.FeedbackRange);
                    feedbackDataProxy.Image =
                        GetHealthySnackImageRessource((FoodAmountType) feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_HealthySnackTitle;

                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.UnhealthySnack:
                    feedbackDataProxy.FeedbackState = GetUnhealthySnackFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    //Round actualvalue after calculation to ensure correct display
                    feedbackDataProxy.ActualValue = Math.Round(feedbackDataProxy.ActualValue, 0);
                    feedbackDataProxy.Recommendation = GetUnhealthyRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image =
                        GetUnhealthySnackImageRessource((FoodAmountType) feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_UnhealthySnackTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.CalorieDrinkAlcohol:

                    feedbackDataProxy.FeedbackState = GetCalorieDrinkFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Recommendation = GetCalorieDrinkRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image = GetCalorieDrinkAlcoholImageResource(feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_CalorieDrinkAlcoholTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.Water:

                    feedbackDataProxy.FeedbackState = GetWaterFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Recommendation = GetWaterRecommendation(feedbackDataProxy.FeedbackState,
                        feedbackDataProxy.FeedbackRange);
                    feedbackDataProxy.Image = GetWaterImageRessource(feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_WaterTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.Exercise:

                    feedbackDataProxy.FeedbackState = GetExerciseFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Recommendation = GetExerciseRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image = GetExerciseImageRessource(feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Title = AppResources.Feedback_ActivityTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                case FeedbackType.Fasting:
                    break;
                case FeedbackType.Relaxation:
                    feedbackDataProxy.FeedbackState = GetMeditationFeedbackState(feedbackDataProxy.RecommendedValue,
                        feedbackDataProxy.ActualValue);
                    feedbackDataProxy.FeedbackRange = GetFeedbackRange(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);
                    feedbackDataProxy.Recommendation = GetMeditationRecommendation(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Image = GetMeditationImageRessource(feedbackDataProxy.FeedbackState);
                    feedbackDataProxy.Title = AppResources.Feedback_MeditationTitle;
                    feedbackDataProxy.Score = GetScore(feedbackDataProxy.RecommendedValue, feedbackDataProxy.ActualValue);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return feedbackDataProxy;
        }

        private string GetMeditationRecommendation(FeedbackState feedbackState)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_MeditationAmountBad;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_MeditationAmountGood;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_MeditationAmountIdeal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private string GetExerciseRecommendation(FeedbackState feedbackState)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_ActivityAmountBad;
                    break;
                case FeedbackState.VeryLow:
                    recommendation = AppResources.Feedback_ActivityAmountBad;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_ActivityAmountGood;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_ActivityAmountGood;
                    break;
                case FeedbackState.LittleHigh:
                    recommendation = AppResources.Feedback_ActivityAmountGreat;
                    break;
                case FeedbackState.VeryHigh:
                    recommendation = AppResources.Feedback_ActivityAmountGreat;
                    break;
                case FeedbackState.TooHigh:
                    recommendation = AppResources.Feedback_ActivityAmountGreat;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private string GetWaterRecommendation(FeedbackState feedbackState, FeedbackRangeType feedbackRange)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_WaterAmountWayLess;
                    break;
                case FeedbackState.VeryLow:
                    recommendation = AppResources.Feedback_WaterAmountMedLess;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_WaterAmountLess;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_WaterAmountIdeal;
                    break;
                case FeedbackState.LittleHigh:
                    recommendation = AppResources.Feedback_WaterAmountMuch;
                    break;
                case FeedbackState.VeryHigh:
                    recommendation = AppResources.Feedback_WaterAmountMedMuch;
                    break;
                case FeedbackState.TooHigh:
                    recommendation = AppResources.Feedback_WaterAmountWayMuch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private string GetCalorieDrinkRecommendation(FeedbackState feedbackState)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountWayLess;
                    break;
                case FeedbackState.VeryLow:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountMedLess;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountLess;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountMatch;
                    break;
                case FeedbackState.LittleHigh:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountMuch;
                    break;
                case FeedbackState.VeryHigh:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountMedMuch;
                    break;
                case FeedbackState.TooHigh:
                    recommendation = AppResources.Feedback_CalorieDrinkAmountWayMuch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private string GetMealRecommendation(FeedbackState feedbackState)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_MealAmountWayLess;
                    break;
                case FeedbackState.VeryLow:
                    recommendation = AppResources.Feedback_MealAmountMedLess;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_MealAmountLess;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_MealAmountIdeal;
                    break;
                case FeedbackState.LittleHigh:
                    recommendation = AppResources.Feedback_MealAmountMuch;
                    break;
                case FeedbackState.VeryHigh:
                    recommendation = AppResources.Feedback_MealAmountMedMuch;
                    break;
                case FeedbackState.TooHigh:
                    recommendation = AppResources.Feedback_MealAmountWayMuch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private string GetHealthySnacksRecommendation(FeedbackState feedbackState, FeedbackRangeType feedbackRange)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_HealthySnackAmountWayLess;
                    break;
                case FeedbackState.VeryLow:
                    recommendation = AppResources.Feedback_HealthySnackAmountMedLess;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_HealthySnackAmountLess;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_HealthySnackAmountIdeal;
                    break;
                case FeedbackState.LittleHigh:
                    recommendation = AppResources.Feedback_HealthySnackAmountMuch;
                    break;
                case FeedbackState.VeryHigh:
                    recommendation = AppResources.Feedback_HealthySnackAmountMedMuch;
                    break;
                case FeedbackState.TooHigh:
                    recommendation = AppResources.Feedback_HealthySnackAmountWayMuch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private string GetUnhealthyRecommendation(FeedbackState feedbackState)
        {
            string recommendation;

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountWayLess;
                    break;
                case FeedbackState.VeryLow:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountMedLess;
                    break;
                case FeedbackState.LittleLow:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountLess;
                    break;
                case FeedbackState.Perfect:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountMatch;
                    break;
                case FeedbackState.LittleHigh:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountMuch;
                    break;
                case FeedbackState.VeryHigh:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountMedMuch;
                    break;
                case FeedbackState.TooHigh:
                    recommendation = AppResources.Feedback_UnhealthySnackAmountWayMuch;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return recommendation;
        }

        private FeedbackState GetMeditationFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.TooLow;

            if (actualValue.Equals(0.0))
            {
                return feedbackState;
            }

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (actualValue < 10.0)
                {
                    feedbackState = FeedbackState.TooLow;
                }
                else if (actualValue >= 10.0)
                {
                    feedbackState = FeedbackState.LittleLow;
                }
            }
            else if (recommendedValue.Equals(actualValue) || recommendedValue < actualValue)
            {
                feedbackState = FeedbackState.Perfect;
            }

            return feedbackState;
        }

        private FeedbackState GetExerciseFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.TooLow;

            if (actualValue.Equals(0.0)) return feedbackState;

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (difference > 1.5)
                    feedbackState = FeedbackState.TooLow;
                else if (difference > 0.5 && difference <= 1.5)
                    feedbackState = FeedbackState.VeryLow;
                else if (difference <= 0.5) feedbackState = FeedbackState.LittleLow;
            }
            else if (recommendedValue.Equals(actualValue))
            {
                feedbackState = FeedbackState.Perfect;
            }
            else if (recommendedValue < actualValue)
            {
                var difference = actualValue - recommendedValue;

                if (difference > 2.5)
                    feedbackState = FeedbackState.TooHigh;
                else if (difference > 1.0 && difference <= 2.5)
                    feedbackState = FeedbackState.VeryHigh;
                else if (difference >= 0.5 && difference <= 1.0) feedbackState = FeedbackState.LittleHigh;
            }

            return feedbackState;
        }

        private FeedbackState GetWaterFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.VeryLow;

            if (actualValue.Equals(0.0)) return feedbackState;

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (difference > 1.5)
                    feedbackState = FeedbackState.TooLow;
                else if (difference > 0.5 && difference <= 1.5)
                    feedbackState = FeedbackState.VeryLow;
                else if (difference <= 0.5) feedbackState = FeedbackState.LittleLow;
            }
            else if (recommendedValue.Equals(actualValue))
            {
                feedbackState = FeedbackState.Perfect;
            }
            else if (recommendedValue < actualValue)
            {
                var difference = actualValue - recommendedValue;

                if (difference > 1.5)
                    feedbackState = FeedbackState.TooHigh;
                else if (difference > 0.5 && difference <= 1.5)
                    feedbackState = FeedbackState.VeryHigh;
                else if (difference > 0 && difference <= 0.5) feedbackState = FeedbackState.LittleHigh;
            }

            return feedbackState;
        }

        private FeedbackState GetCalorieDrinkFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.TooLow;

            if (actualValue.Equals(0.0)) return feedbackState;

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (difference > 1.0)
                    feedbackState = FeedbackState.TooLow;
                else if (difference > 0.5 && difference <= 1.0)
                    feedbackState = FeedbackState.VeryLow;
                else if (difference <= 0.5) feedbackState = FeedbackState.LittleLow;
            }
            else if (recommendedValue.Equals(actualValue))
            {
                feedbackState = FeedbackState.Perfect;
            }
            else if (recommendedValue < actualValue)
            {
                var difference = actualValue - recommendedValue;

                if (difference > 1.0)
                    feedbackState = FeedbackState.TooHigh;
                else if (difference > 0.5 && difference <= 1.0)
                    feedbackState = FeedbackState.VeryHigh;
                else if (difference > 0 && difference <= 0.5) feedbackState = FeedbackState.LittleHigh;
            }

            return feedbackState;
        }

        private FeedbackState GetMealFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.TooLow;

            if (actualValue.Equals(-1.0)) return feedbackState;

            actualValue = Math.Round(actualValue, 0);

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (difference.Equals(1.0))
                    feedbackState = FeedbackState.LittleLow;
                else if (difference.Equals(2.0))
                    feedbackState = FeedbackState.VeryLow;
                else if (difference > 2.0) feedbackState = FeedbackState.TooLow;
            }
            else if (recommendedValue.Equals(actualValue))
            {
                feedbackState = FeedbackState.Perfect;
            }
            else if (recommendedValue < actualValue)
            {
                var difference = actualValue - recommendedValue;

                if (difference.Equals(1.0))
                    feedbackState = FeedbackState.LittleHigh;
                else if (difference.Equals(2.0))
                    feedbackState = FeedbackState.VeryHigh;
                else if (difference > 2.0) feedbackState = FeedbackState.TooHigh;
            }

            return feedbackState;
        }

        private FeedbackState GetHealthySnackFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.TooLow;

            if (actualValue.Equals(-1.0)) return feedbackState;

            actualValue = Math.Round(actualValue, 0);

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (difference.Equals(1.0))
                    feedbackState = FeedbackState.LittleLow;
                else if (difference.Equals(2.0))
                    feedbackState = FeedbackState.VeryLow;
                else if (difference > 2.0) feedbackState = FeedbackState.TooLow;
            }
            else if (recommendedValue.Equals(actualValue))
            {
                feedbackState = FeedbackState.Perfect;
            }
            else if (recommendedValue < actualValue)
            {
                var difference = actualValue - recommendedValue;

                if (difference.Equals(1.0))
                    feedbackState = FeedbackState.LittleHigh;
                else if (difference.Equals(2.0))
                    feedbackState = FeedbackState.VeryHigh;
                else if (difference > 2.0) feedbackState = FeedbackState.TooHigh;
            }

            return feedbackState;
        }

        private FeedbackState GetUnhealthySnackFeedbackState(double recommendedValue, double actualValue)
        {
            var feedbackState = FeedbackState.LittleHigh;

            if (actualValue.Equals(-1.0)) return feedbackState;

            actualValue = Math.Round(actualValue, 0);

            if (recommendedValue > actualValue)
            {
                var difference = recommendedValue - actualValue;

                if (difference.Equals(1.0))
                    feedbackState = FeedbackState.LittleLow;
                else if (difference.Equals(2.0))
                    feedbackState = FeedbackState.VeryLow;
                else if (difference > 2.0) feedbackState = FeedbackState.TooLow;
            }
            else if (recommendedValue.Equals(actualValue))
            {
                feedbackState = FeedbackState.Perfect;
            }
            else if (recommendedValue < actualValue)
            {
                var difference = actualValue - recommendedValue;

                if (difference.Equals(1.0))
                    feedbackState = FeedbackState.LittleHigh;
                else if (difference.Equals(2.0))
                    feedbackState = FeedbackState.VeryHigh;
                else if (difference > 2.0) feedbackState = FeedbackState.TooHigh;
            }

            return feedbackState;
        }

        private FeedbackRangeType GetFeedbackRange(double recommendedValue, double actualValue)
        {
            var feedbackRange = FeedbackRangeType.Matching;

            if (recommendedValue > actualValue)
                feedbackRange = FeedbackRangeType.Below;
            else if (recommendedValue < actualValue) feedbackRange = FeedbackRangeType.Above;

            return feedbackRange;
        }

        private int  GetScore(double totalValue, double actualValue)
        {
            int retvalue = 0;
            if (totalValue > 0)
            {
                retvalue =Convert.ToInt32( Math.Floor( (100 * actualValue) / totalValue));
            }
            return retvalue;
        }

        //private ImageSource GetExerciseImageResource(FeedbackState feedbackState)
        //{
        //    var imageResource = "ZeroGravity.Mobile.Resources.Images.";

        //    switch (feedbackState)
        //    {
        //        case FeedbackState.TooLow:
        //            imageResource += "exercise_1.png";
        //            break;
        //        case FeedbackState.VeryLow:
        //            imageResource += "exercise_2.png";
        //            break;
        //        case FeedbackState.LittleLow:
        //            imageResource += "exercise_3.png";
        //            break;
        //        case FeedbackState.Perfect:
        //            imageResource += "exercise_4.png";
        //            break;
        //        case FeedbackState.LittleHigh:
        //            imageResource += "exercise_5.png";
        //            break;
        //        case FeedbackState.VeryHigh:
        //            imageResource += "exercise_6.png";
        //            break;
        //        case FeedbackState.TooHigh:
        //            imageResource += "exercise_6.png";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
        //    }

        //    return ImageSource.FromResource(imageResource);
        //}

        private ImageSource GetExerciseImageRessource(double actualAmount)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            if (actualAmount <= 0.5)
                imageResource += "exercise_1.png";
            else if (actualAmount > 0.5 && actualAmount <= 1.0)
                imageResource += "exercise_2.png";
            else if (actualAmount > 1.0 && actualAmount <= 1.5)
                imageResource += "exercise_3.png";
            else if (actualAmount > 1.5 && actualAmount <= 2.0)
                imageResource += "exercise_4.png";
            else if (actualAmount > 2.0 && actualAmount <= 2.5)
                imageResource += "exercise_5.png";
            else if (actualAmount > 2.5) imageResource += "exercise_6.png";

            return ImageSource.FromResource(imageResource);
        }

        //private ImageSource GetWaterImageRessource(FeedbackState feedbackState)
        //{
        //    var imageResource = "ZeroGravity.Mobile.Resources.Images.";

        //    switch (feedbackState)
        //    {
        //        case FeedbackState.TooLow:
        //            imageResource += "waterintake_1.png";
        //            break;
        //        case FeedbackState.VeryLow:
        //            imageResource += "waterintake_2.png";
        //            break;
        //        case FeedbackState.LittleLow:
        //            imageResource += "waterintake_3.png";
        //            break;
        //        case FeedbackState.Perfect:
        //            imageResource += "waterintake_4.png";
        //            break;
        //        case FeedbackState.LittleHigh:
        //            imageResource += "waterintake_5.png";
        //            break;
        //        case FeedbackState.VeryHigh:
        //            imageResource += "waterintake_6.png";
        //            break;
        //        case FeedbackState.TooHigh:
        //            imageResource += "waterintake_6.png";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
        //    }

        //    return ImageSource.FromResource(imageResource);
        //}

        private ImageSource GetWaterImageRessource(double actualAmount)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            if (actualAmount <= 0.5)
                imageResource += "waterintake_1.png";
            else if (actualAmount > 0.5 && actualAmount <= 1.5)
                imageResource += "waterintake_2.png";
            else if (actualAmount > 1.5 && actualAmount <= 2.5)
                imageResource += "waterintake_3.png";
            else if (actualAmount > 2.5 && actualAmount <= 3.5)
                imageResource += "waterintake_4.png";
            else if (actualAmount > 3.5 && actualAmount <= 4.5)
                imageResource += "waterintake_5.png";
            else if (actualAmount > 4.5) imageResource += "waterintake_6.png";

            return ImageSource.FromResource(imageResource);
        }

        //private ImageSource GetCalorieDrinkAlcoholImageResource(FeedbackState feedbackState)
        //{
        //    var imageResource = "ZeroGravity.Mobile.Resources.Images.";

        //    switch (feedbackState)
        //    {
        //        case FeedbackState.TooHigh:
        //            imageResource += "caloriedrinks_1.png";
        //            break;
        //        case FeedbackState.VeryHigh:
        //            imageResource += "caloriedrinks_2.png";
        //            break;
        //        case FeedbackState.LittleHigh:
        //            imageResource += "caloriedrinks_3.png";
        //            break;
        //        case FeedbackState.Perfect:
        //            imageResource += "caloriedrinks_4.png";
        //            break;
        //        case FeedbackState.LittleLow:
        //            imageResource += "caloriedrinks_5.png";
        //            break;
        //        case FeedbackState.VeryLow:
        //            imageResource += "caloriedrinks_6.png";
        //            break;
        //        case FeedbackState.TooLow:
        //            imageResource += "caloriedrinks_6.png";
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
        //    }

        //    return ImageSource.FromResource(imageResource);
        //}

        private ImageSource GetCalorieDrinkAlcoholImageResource(double actualAmount)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            if (actualAmount <= 0.5)
                imageResource += "caloriedrinks_6.png";
            else if (actualAmount > 0.5 && actualAmount <= 1.0)
                imageResource += "caloriedrinks_5.png";
            else if (actualAmount > 1.0 && actualAmount <= 1.5)
                imageResource += "caloriedrinks_4.png";
            else if (actualAmount > 1.5 && actualAmount <= 2.0)
                imageResource += "caloriedrinkss_3.png";
            else if (actualAmount > 2.0 && actualAmount <= 2.5)
                imageResource += "caloriedrinks_2.png";
            else if (actualAmount > 2.5) imageResource += "caloriedrinks_1.png";

            return ImageSource.FromResource(imageResource);
        }

        private ImageSource GetMeditationImageRessource(FeedbackState feedbackState)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            switch (feedbackState)
            {
                case FeedbackState.TooLow:
                    imageResource += "meditation_1.png";
                    break;
                case FeedbackState.LittleLow:
                    imageResource += "meditation_2.png";
                    break;
                case FeedbackState.Perfect:
                    imageResource += "meditation_3.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedbackState), feedbackState, null);
            }

            return ImageSource.FromResource(imageResource);
        }

        private ImageSource GetBreakfastImageRessource(FoodAmountType foodAmount)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            switch (foodAmount)
            {
                case FoodAmountType.VeryHeavy:
                    imageResource += "breakfast_5.png";
                    break;
                case FoodAmountType.Heavy:
                    imageResource += "breakfast_4.png";
                    break;
                case FoodAmountType.Medium:
                    imageResource += "breakfast_3.png";
                    break;
                case FoodAmountType.Light:
                    imageResource += "breakfast_2.png";
                    break;
                case FoodAmountType.VeryLight:
                    imageResource += "breakfast_1.png";
                    break;
                case FoodAmountType.None:
                    imageResource += "breakfast_1.png";
                    break;
                case FoodAmountType.Undefined:
                    imageResource += "breakfast_1.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(foodAmount), foodAmount, null);
            }

            return ImageSource.FromResource(imageResource);
        }

        private ImageSource GetLunchImageRessource(FoodAmountType foodAmountType)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            switch (foodAmountType)
            {
                case FoodAmountType.VeryHeavy:
                    imageResource += "lunch_5.png";
                    break;
                case FoodAmountType.Heavy:
                    imageResource += "lunch_4.png";
                    break;
                case FoodAmountType.Medium:
                    imageResource += "lunch_3.png";
                    break;
                case FoodAmountType.Light:
                    imageResource += "lunch_2.png";
                    break;
                case FoodAmountType.VeryLight:
                    imageResource += "lunch_1.png";
                    break;
                case FoodAmountType.None:
                    imageResource += "lunch_1.png";
                    break;
                case FoodAmountType.Undefined:
                    imageResource += "lunch_1.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(foodAmountType), foodAmountType, null);
            }

            return ImageSource.FromResource(imageResource);
        }

        private ImageSource GetDinnerImageRessource(FoodAmountType foodAmount)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            switch (foodAmount)
            {
                case FoodAmountType.VeryHeavy:
                    imageResource += "dinner_5.png";
                    break;
                case FoodAmountType.Heavy:
                    imageResource += "dinner_4.png";
                    break;
                case FoodAmountType.Medium:
                    imageResource += "dinner_3.png";
                    break;
                case FoodAmountType.Light:
                    imageResource += "dinner_2.png";
                    break;
                case FoodAmountType.VeryLight:
                    imageResource += "dinner_1.png";
                    break;
                case FoodAmountType.None:
                    imageResource += "dinner_1.png";
                    break;
                case FoodAmountType.Undefined:
                    imageResource += "dinner_1.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(foodAmount), foodAmount, null);
            }

            return ImageSource.FromResource(imageResource);
        }

        private ImageSource GetHealthySnackImageRessource(FoodAmountType amountType)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            switch (amountType)
            {
                case FoodAmountType.VeryLight:
                    imageResource += "healthysnack_1.png";
                    break;
                case FoodAmountType.Light:
                    imageResource += "healthysnack_2.png";
                    break;
                case FoodAmountType.Medium:
                    imageResource += "healthysnack_3.png";
                    break;
                case FoodAmountType.Heavy:
                    imageResource += "healthysnack_4.png";
                    break;
                case FoodAmountType.VeryHeavy:
                    imageResource += "healthysnack_5.png";
                    break;
                case FoodAmountType.None:
                    imageResource += "healthysnack_1.png";
                    break;
                case FoodAmountType.Undefined:
                    imageResource += "healthysnack_1.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(amountType), amountType, null);
            }

            return ImageSource.FromResource(imageResource);
        }

        private ImageSource GetUnhealthySnackImageRessource(FoodAmountType foodAmount)
        {
            var imageResource = "ZeroGravity.Mobile.Resources.Images.";

            switch (foodAmount)
            {
                case FoodAmountType.VeryLight:
                    imageResource += "unhealthysnack_5.png";
                    break;
                case FoodAmountType.Light:
                    imageResource += "unhealthysnack_4.png";
                    break;
                case FoodAmountType.Medium:
                    imageResource += "unhealthysnack_3.png";
                    break;
                case FoodAmountType.Heavy:
                    imageResource += "unhealthysnack_2.png";
                    break;
                case FoodAmountType.VeryHeavy:
                    imageResource += "unhealthysnack_1.png";
                    break;
                case FoodAmountType.None:
                    imageResource += "unhealthysnack_5.png";
                    break;
                case FoodAmountType.Undefined:
                    imageResource += "unhealthysnack_5.png";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(foodAmount), foodAmount, null);
            }

            return ImageSource.FromResource(imageResource);
        }
    }
}