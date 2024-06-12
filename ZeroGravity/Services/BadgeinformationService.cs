using System;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class BadgeinformationService : IBadgeinformationService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public BadgeinformationService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public async Task<ActivityBadgeInformationDto> GetActivityBadgeInformationAsync(int accountId, DateTime dateTime)
        {
            var activityBadgeInformationDto = new ActivityBadgeInformationDto();

            var numberOfExerciseActivities = await _repository.Execute(new GetActivityNumberByAccoundId(accountId, dateTime, ActivityType.Exercise));
            var numberOfDayToDayActivities = await _repository.Execute(new GetActivityNumberByAccoundId(accountId, dateTime, ActivityType.DayToDay));

            activityBadgeInformationDto.ExerciseNumber = numberOfExerciseActivities;
            activityBadgeInformationDto.DayToDayNumber = numberOfDayToDayActivities;

            return activityBadgeInformationDto;
        }

        public async Task<MealsBadgeInformationDto> GetMealsBadgeInformationAsync(int accountId, DateTime dateTime)
        {
            // TODO Need to Improve Performance. Why to fetch individually ? can improve query by combining

            var activityBadgeInformationDto = new MealsBadgeInformationDto
            {
                TotalAmount = await _repository.Execute(new GetMealTotalCountByAccountId(accountId, dateTime))
            };

            //var numberOfHealthySnacks = await _repository.Execute(new GetMealNumberByAccountId(accountId, dateTime, MealSlotType.HealthySnack));
            //var numberOfUnhealthySnacks = await _repository.Execute(new GetMealNumberByAccountId(accountId, dateTime, MealSlotType.UnhealthySnack));
            //var foodAmountBreakfast = await _repository.Execute(new GetMealAmountByAccountId(accountId, dateTime, MealSlotType.Breakfast));
            //var foodAmountLunch = await _repository.Execute(new GetMealAmountByAccountId(accountId, dateTime, MealSlotType.Lunch));
            //var foodAmountDinner = await _repository.Execute(new GetMealAmountByAccountId(accountId, dateTime, MealSlotType.Dinner));

            //activityBadgeInformationDto.HealthySnackNumber = numberOfHealthySnacks;
            //activityBadgeInformationDto.UnhealthySnackNumber = numberOfUnhealthySnacks;
            //activityBadgeInformationDto.BreakfastAmount = foodAmountBreakfast;
            //activityBadgeInformationDto.LunchAmount = foodAmountLunch;
            //activityBadgeInformationDto.DinnerAmount = foodAmountDinner;
            // activityBadgeInformationDto.TotalAmount = total;

            return activityBadgeInformationDto;
        }

        public async Task<LiquidIntakeBadgeInformationDto> GetLiquidIntakeBadgeInformationAsync(int accountId, DateTime dateTime)
        {
            var liquidIntakeBadgeInformationDto = new LiquidIntakeBadgeInformationDto();

            var numberOfWaterIntakes = await _repository.Execute(new GetLiquidIntakeNumberByAccountId(accountId, dateTime, LiquidType.Water));
            var numberOfCalorieDrinks = await _repository.Execute(new GetLiquidIntakeNumberByAccountId(accountId, dateTime, LiquidType.CalorieDrinkAndAlcohol));

            liquidIntakeBadgeInformationDto.WaterIntakeNumber = numberOfWaterIntakes;
            liquidIntakeBadgeInformationDto.CalorieDrinksAlcoholNumber = numberOfCalorieDrinks;

            return liquidIntakeBadgeInformationDto;
        }

        public async Task<WellbeingType> GetWellbeingBadgeInformationAsync(int accountId, DateTime dateTime)
        {
            var wellbeingData = await _repository.Execute(new GetWellbeingDataByAccountId(accountId, dateTime));

            if (wellbeingData != null)
            {
                return wellbeingData.Rating;
            }

            return WellbeingType.VeryBad;
        }

        public async Task<MyDayBadgeInformationDto> GetMyDayBadgeInformationAsync(int accountId, DateTime dateTime)
        {
            var myDayBadgeInformationDto = new MyDayBadgeInformationDto();

            var liquidIntakeBadgeInformationDto = await GetLiquidIntakeBadgeInformationAsync(accountId, dateTime);
            var activityBadgeInformationDto = await GetActivityBadgeInformationAsync(accountId, dateTime);
            var mealsBadgeInformationDto = await GetMealsBadgeInformationAsync(accountId, dateTime);
            var wellbeingBadgeInformation = await GetWellbeingBadgeInformationAsync(accountId, dateTime);

            myDayBadgeInformationDto.LiquidIntakeBadgeInformationDto = liquidIntakeBadgeInformationDto;
            myDayBadgeInformationDto.ActivityBadgeInformationDto = activityBadgeInformationDto;
            myDayBadgeInformationDto.MealsBadgeInformationDto = mealsBadgeInformationDto;
            myDayBadgeInformationDto.WellbeingBadgeInformation = wellbeingBadgeInformation;

            return myDayBadgeInformationDto;
        }
    }
}