using System;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public FeedbackService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public async Task<FeedbackSummaryDto> GetFeedbackByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            var personalGoals = await _repository.Execute(new GetPersonalGoalsByAccountId(accountId));

            var personalData = await _repository.Execute(new GetPersonalDataByAccountId(accountId));

            //Get the date from a week back
            var initalDateTime = dateTime.AddDays(-7);

            var liquidIntakes = await _repository.Execute(new GetLiquidIntakeFromDatespan(accountId, initalDateTime, dateTime));

            var activities = await _repository.Execute(new GetActivityFromDatespan(accountId, initalDateTime, dateTime));

            var meals = await _repository.Execute(new GetMealFromDatespan(accountId, initalDateTime, dateTime));

            var meditationDatas = await _repository.Execute(new GetMeditationByDatespan(accountId, initalDateTime, dateTime));

            var meditationAverage = Math.Round(meditationDatas.Sum(_ => _.Duration.TotalMinutes) / 7, 1);

            var activityAverage = Math.Round(activities.Where(_ => _.ActivityType == ActivityType.Exercise).Sum(_ => _.Duration.TotalHours) / 7, 1);

            var targetWaterIntakeAverage = Math.Round(liquidIntakes.Where(_ => _.LiquidType == LiquidType.Water).Sum(_ => _.AmountMl) / 1000, 3); 
            
            targetWaterIntakeAverage = Math.Round(targetWaterIntakeAverage / 7, 2);

            var targetCalorieDrinkIntakeAverage = Math.Round(liquidIntakes.Where(_ => _.LiquidType == LiquidType.CalorieDrinkAndAlcohol).Sum(_ => _.AmountMl) / 1000, 3);

            targetCalorieDrinkIntakeAverage = Math.Round(targetCalorieDrinkIntakeAverage / 7, 2);

            var breakfastList = meals.Where(_ => _.MealSlotType.Equals(MealSlotType.Breakfast)).ToList();
            var lunchList = meals.Where(_ => _.MealSlotType.Equals(MealSlotType.Lunch)).ToList();
            var dinnerList = meals.Where(_ => _.MealSlotType.Equals(MealSlotType.Dinner)).ToList();

            var healthyList = meals.Where(_ => _.MealSlotType.Equals(MealSlotType.HealthySnack)).ToList();
            var unhealthyList = meals.Where(_ => _.MealSlotType.Equals(MealSlotType.UnhealthySnack)).ToList();

            var breakfastAverage =  breakfastList.Any() ? Math.Round(breakfastList.Sum(_ => (double) _.Amount) / 7, 1) : 0;
            var lunchAverage =  lunchList.Any() ? Math.Round(lunchList.Sum(_ => (double) _.Amount) / 7, 1) : 0;
            var dinnerAverage = dinnerList.Any() ? Math.Round(dinnerList.Sum(_ => (double) _.Amount) / 7, 1) : 0;

            var healthyAverage =  healthyList.Any() ? Math.Round(healthyList.Sum(_ => (double) _.Amount) / 7, 1) : 0;
            var unhealthyAverage =  unhealthyList.Any() ? Math.Round(unhealthyList.Sum(_ => (double) _.Amount) / 7, 1) : 0;

            var activityFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.Exercise,
                RecommendedValue = personalGoals.ActivityDuration,
                ActualValue = activityAverage
            };

            var waterIntakeFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.Water,
                RecommendedValue = personalGoals.WaterConsumption,
                ActualValue = targetWaterIntakeAverage
            };

            var calorieDrinkIntakeFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.CalorieDrinkAlcohol,
                RecommendedValue = personalGoals.CalorieDrinkAlcoholConsumption,
                ActualValue = targetCalorieDrinkIntakeAverage
            };

            var breakfastFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.Breakfast,
                RecommendedValue = (double) personalGoals.BreakfastAmount,
                ActualValue = breakfastAverage
            };

            var lunchFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.Lunch,
                RecommendedValue = (double) personalGoals.LunchAmount,
                ActualValue = lunchAverage
            };

            var dinnerFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.Dinner,
                RecommendedValue = (double) personalGoals.DinnerAmount,
                ActualValue = dinnerAverage
            };

            var healthySnackFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.HealthySnack,
                RecommendedValue = (double) personalGoals.HealthySnackAmount,
                ActualValue = healthyAverage
            };

            var unhealthyFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.UnhealthySnack,
                RecommendedValue = (double) personalGoals.UnhealthySnackAmount,
                ActualValue = unhealthyAverage
            };

            var meditationFeedback = new FeedbackDataDto
            {
                FeedbackType = FeedbackType.Relaxation,
                RecommendedValue = 20.0,
                ActualValue = meditationAverage
            };

            var personalDataDto = DtoConverter.GetPersonalDataDto(personalData);

            var feedbackSummary = new FeedbackSummaryDto
            {
                PersonalDataDto = personalDataDto,
                ActivityFeedbackDataDto = activityFeedback,
                CalorieDrinkFeedbackDataDto = calorieDrinkIntakeFeedback,
                WaterFeedbackDataDto = waterIntakeFeedback,
                BreakfastFeedbackDataDto = breakfastFeedback,
                LunchFeedbackDataDto = lunchFeedback,
                DinnerFeedbackDataDto = dinnerFeedback,
                HealthyFeedbackDataDto = healthySnackFeedback,
                UnhealthyFeedbackDataDto = unhealthyFeedback,
                MeditationDataDto = meditationFeedback
            };

            return feedbackSummary;
        }
    }
}