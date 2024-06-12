using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Queries;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class TrackedHistoryService : ITrackedHistorieService
    {
        private readonly IActivityService _activityService;
        private readonly ILiquidIntakeService _liquidIntakeService;
        private readonly IMealDataService _mealDataService;
        private readonly IWellbeingDataService _wellbeingDataService;

        public TrackedHistoryService(IActivityService activityService, ILiquidIntakeService liquidIntakeService, IMealDataService mealDataService, IWellbeingDataService wellbeingDataService)
        {
            _activityService = activityService;
            _liquidIntakeService = liquidIntakeService;
            _mealDataService = mealDataService;
            _wellbeingDataService = wellbeingDataService;
        }

        public async Task<List<TrackedHistoryDto>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            List<TrackedHistoryDto> trackedHistoryDtos = new List<TrackedHistoryDto>();

            var activityDataList = await _activityService.GetAllByAccountIdAndDateAsync(accountId, dateTime);

            foreach (var activityData in activityDataList)
            {
                trackedHistoryDtos.Add(new TrackedHistoryDto
                {
                    Id = activityData.Id,
                    AccountId = activityData.AccountId,
                    HistoryItemType = HistoryItemType.Activity,
                    Created = activityData.Created,
                    Duration = activityData.Duration.TotalMinutes.ToString()
                });
            }

            var mealDataList = await _mealDataService.GetByAccoundAndDateAsync(accountId, dateTime);

            foreach (var mealData in mealDataList)
            {
                var historyItem = new TrackedHistoryDto
                {
                    Id = mealData.Id,
                    AccountId = mealData.AccountId,
                    Created = mealData.Created,
                    FoodAmountType = mealData.Amount
                };

                switch (mealData.MealSlotType)
                {
                    case MealSlotType.Breakfast:
                        historyItem.HistoryItemType = HistoryItemType.Breakfast;
                        break;

                    case MealSlotType.Lunch:
                        historyItem.HistoryItemType = HistoryItemType.Lunch;
                        break;

                    case MealSlotType.Dinner:
                        historyItem.HistoryItemType = HistoryItemType.Dinner;
                        break;

                    case MealSlotType.HealthySnack:
                        historyItem.HistoryItemType = HistoryItemType.HealthySnack;
                        break;

                    case MealSlotType.UnhealthySnack:
                        historyItem.HistoryItemType = HistoryItemType.UnhealthySnack;
                        break;
                }

                trackedHistoryDtos.Add(historyItem);
            }

            var liquidIntake = await _liquidIntakeService.GetByAccoundAndDateAsync(accountId, dateTime);

            foreach (var intake in liquidIntake)
            {
                var historyItem = new TrackedHistoryDto
                {
                    Id = intake.Id,
                    AccountId = intake.AccountId,
                    Created = intake.Created,
                    Amount = intake.AmountMl
                };

                switch (intake.LiquidType)
                {
                    case LiquidType.Water:
                        historyItem.HistoryItemType = HistoryItemType.WaterIntake;
                        break;

                    case LiquidType.CalorieDrinkAndAlcohol:
                        historyItem.HistoryItemType = HistoryItemType.CalorieDrinkAlcohol;
                        break;
                }

                trackedHistoryDtos.Add(historyItem);
            }

            var wellbeings = await _wellbeingDataService.GetAllByAccountIdAndDateAsync(accountId, dateTime);

            if (wellbeings != null)
            {
                foreach (var wellbeing in wellbeings)
                {
                    var historyItem1 = new TrackedHistoryDto
                    {
                        Id = wellbeing.Id,
                        AccountId = wellbeing.AccountId,
                        Created = wellbeing.Created,
                        WellbeingType = wellbeing.Rating,
                        HistoryItemType = HistoryItemType.Wellbeing
                    };

                    trackedHistoryDtos.Add(historyItem1);
                }
            }

            return trackedHistoryDtos;
        }
    }
}