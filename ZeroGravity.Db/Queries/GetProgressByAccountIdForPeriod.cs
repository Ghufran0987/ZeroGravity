using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoredProcedureEFCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetProgressByAccountIdForPeriod : IDbQuery<List<ProgressSummaryDto>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;

        public GetProgressByAccountIdForPeriod(int accountId, DateTime fromDate, DateTime toDate)
        {
            _accountId = accountId;
            _fromDate = fromDate;
            _toDate = toDate + QueryConstants.DayDuration;
        }

        public async Task<List<ProgressSummaryDto>> Execute(ZeroGravityContext dbContext)
        {
            List<ProgressSummaryDto> results = new List<ProgressSummaryDto>();
            PersonalGoal goal = await dbContext.PersonalGoals.FirstOrDefaultAsync(g => g.AccountId == _accountId);
            if(goal != null)
            {
                // Retrieve data in date range
                List<MealData> mealData = await dbContext.MealDatas.Where(md =>
                    md.AccountId == _accountId
                    && md.Created.Date >= _fromDate.Date
                    && md.Created.Date <= _toDate
                ).ToListAsync();

                List<LiquidIntake> liquidIntakes = await dbContext.LiquidIntakes.Where(li =>
                    li.AccountId == _accountId
                    && li.Created.Date >= _fromDate.Date
                    && li.Created.Date <= _toDate
                ).ToListAsync();

                List<ActivityData> activities = await dbContext.ActivityDatas.Where(a =>
                    a.AccountId == _accountId
                    && a.Created.Date >= _fromDate.Date
                    && a.Created.Date <= _toDate
                ).ToListAsync();

                List<WellbeingData> wellbeingData = await dbContext.WellbeingDatas.Where(w =>
                    w.AccountId == _accountId
                    && w.Created.Date >= _fromDate.Date
                    && w.Created.Date <= _toDate
                ).ToListAsync();

                int breakfastTotalDays = 0,
                    breakfastTotalDaysGoalAchieved = 0,
                    lunchTotalDays = 0,
                    lunchTotalDaysGoalAchieved = 0,
                    dinnerTotalDays = 0,
                    dinnerTotalDaysGoalAchieved = 0,
                    healthySnackTotalDays = 0,
                    healthySnackTotalDaysGoalAchieved = 0,
                    unhealthySnackTotalDays = 0,
                    unhealthySnackTotalDaysGoalAchieved = 0,
                    waterTotalDays = 0,
                    waterTotalDaysGoalAchieved = 0,
                    calorieDrinkTotalDays = 0,
                    calorieDrinkTotalDaysGoalAchieved = 0,
                    activityTotalDays = 0,
                    activityTotalDaysGoalAchieved = 0,
                    wellbeingTotalDays = 0,
                    wellbeingTotalDaysGoalAchieved = 0;

                // Loop through all days between range
                for (DateTime day = _fromDate.Date; day.Date <= _toDate.Date; day = day.AddDays(1))
                {
                    // Breakfast
                    calculateMealTotalsByDay(
                        mealData,
                        MealSlotType.Breakfast,
                        day,
                        (int)goal.BreakfastAmount,
                        ref breakfastTotalDays,
                        ref breakfastTotalDaysGoalAchieved
                    );

                    // Lunch
                    calculateMealTotalsByDay(
                        mealData,
                        MealSlotType.Lunch,
                        day,
                        (int)goal.LunchAmount,
                        ref lunchTotalDays,
                        ref lunchTotalDaysGoalAchieved
                    );

                    // Dinner
                    calculateMealTotalsByDay(
                        mealData,
                        MealSlotType.Dinner,
                        day,
                        (int)goal.DinnerAmount,
                        ref dinnerTotalDays,
                        ref dinnerTotalDaysGoalAchieved
                    );

                    // Healthy snacks
                    List<MealData> healthySnacksFiltered = mealData.Where(m => m.MealSlotType == MealSlotType.HealthySnack && m.Created.Date == day).ToList();
                    if (healthySnacksFiltered.Count > 0)
                    {
                        healthySnackTotalDays++;
                        int totalAmount = healthySnacksFiltered.Sum(s => s.Quantity);
                        if (totalAmount <= (int)goal.HealthySnackAmount)
                        {
                            healthySnackTotalDaysGoalAchieved++;
                        }
                    }

                    // Unhealthy snacks
                    List<MealData> unhealthySnacksFiltered = mealData.Where(m => m.MealSlotType == MealSlotType.UnhealthySnack && m.Created.Date == day).ToList();
                    if (unhealthySnacksFiltered.Count > 0)
                    {
                        unhealthySnackTotalDays++;
                        int totalAmount = unhealthySnacksFiltered.Sum(s => s.Quantity);
                        if (totalAmount <= (int)goal.UnhealthySnackAmount)
                        {
                            unhealthySnackTotalDaysGoalAchieved++;
                        }
                    }

                    // Water
                    List<LiquidIntake> waterFiltered = liquidIntakes.Where(l => l.LiquidType == LiquidType.Water && l.Created.Date == day).ToList();
                    if (waterFiltered.Count > 0)
                    {
                        waterTotalDays++;
                        int totalAmount = waterFiltered.Sum(m => (int)m.AmountMl);
                        if (totalAmount >= goal.WaterConsumption) // Greater than or equal for water
                        {
                            waterTotalDaysGoalAchieved++;
                        }
                    }

                    // Calorie drinks
                    List<LiquidIntake> calorieDrinksFiltered = liquidIntakes.Where(l => l.LiquidType == LiquidType.CalorieDrinkAndAlcohol && l.Created.Date == day).ToList();
                    if (calorieDrinksFiltered.Count > 0)
                    {
                        calorieDrinkTotalDays++;
                        int totalAmount = calorieDrinksFiltered.Sum(m => (int)m.AmountMl);
                        if (totalAmount <= goal.CalorieDrinkAlcoholConsumption) // Less than or equal for calorie drinks
                        {
                            calorieDrinkTotalDaysGoalAchieved++;
                        }
                    }

                    // Activities
                    List<ActivityData> activitiesFiltered = activities.Where(a => a.Created.Date == day).ToList();
                    if (activitiesFiltered.Count > 0)
                    {
                        activityTotalDays++;
                        int totalAmount = activitiesFiltered.Sum(a => (int)a.Duration.TotalMinutes);
                        if (totalAmount >= goal.ActivityDuration) // Greater than or equal for activities
                        {
                            activityTotalDaysGoalAchieved++;
                        }
                    }

                    // Wellbeing data
                    List<WellbeingData> wellbeingDataFiltered = wellbeingData.Where(w => w.Created.Date == day).ToList();
                    if (wellbeingDataFiltered.Count > 0)
                    {
                        wellbeingTotalDays++;
                        // Take their last entry in the day as the final wellbeing value to compare
                        WellbeingData wellbeing = wellbeingDataFiltered.OrderByDescending(w => w.Created).FirstOrDefault();
                        if (wellbeing != null && wellbeing.Rating >= WellbeingType.Great)
                        {
                            wellbeingTotalDaysGoalAchieved++;
                        }
                    }
                }

                results.Add(new ProgressSummaryDto()
                {
                    Category = "Breakfast",
                    TotalDaysTracked = breakfastTotalDays,
                    TotalDaysGoalAchieved = breakfastTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "Lunch",
                    TotalDaysTracked = lunchTotalDays,
                    TotalDaysGoalAchieved = lunchTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "Dinner",
                    TotalDaysTracked = dinnerTotalDays,
                    TotalDaysGoalAchieved = dinnerTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "HealthySnacks",
                    TotalDaysTracked = healthySnackTotalDays,
                    TotalDaysGoalAchieved = healthySnackTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "UnhealthySnacks",
                    TotalDaysTracked = unhealthySnackTotalDays,
                    TotalDaysGoalAchieved = unhealthySnackTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "Water",
                    TotalDaysTracked = waterTotalDays,
                    TotalDaysGoalAchieved = waterTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "CalorieDrinks",
                    TotalDaysTracked = calorieDrinkTotalDays,
                    TotalDaysGoalAchieved = calorieDrinkTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "Activities",
                    TotalDaysTracked = activityTotalDays,
                    TotalDaysGoalAchieved = activityTotalDaysGoalAchieved
                });

                results.Add(new ProgressSummaryDto()
                {
                    Category = "Wellbeing",
                    TotalDaysTracked = wellbeingTotalDays,
                    TotalDaysGoalAchieved = wellbeingTotalDaysGoalAchieved
                });
            }

            return results;
        }

        // Breakfast/lunch/dinner
        private void calculateMealTotalsByDay(List<MealData> mealData, MealSlotType mealType, DateTime day, int goal, ref int totalDays, ref int totalDaysGoalAchieved)
        {
            List<MealData> mealsFiltered = mealData.Where(m => m.MealSlotType == mealType && m.Created.Date == day).ToList();
            if (mealsFiltered.Count > 0)
            {
                totalDays++;
                int totalAmount = mealsFiltered.Sum(m => (int)m.Amount);
                if (totalAmount <= goal)
                {
                    totalDaysGoalAchieved++;
                }
            }
        }
    }
}