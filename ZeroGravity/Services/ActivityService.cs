using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Helpers;
using System.Linq;
using ZeroGravity.Constants;

namespace ZeroGravity.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<ZeroGravityContext> _repository;

        public ActivityService(IRepository<ZeroGravityContext> repository)
        {
            _repository = repository;
        }

        public Task<ActivityData> GetByIdAsync(int id)
        {
            var activity = _repository.Execute(new GetActivityById(id));

            return activity;
        }

        public Task<List<ActivityData>> GetAllByAccountIdAndDateAsync(int accountId, DateTime dateTime)
        {
            var activityDataList = _repository.Execute(new GetAllActivitiesByAccountId(accountId, dateTime));
            return activityDataList;
        }

        public Task<List<ActivityData>> GetAllByAccountIdAndDateRangeAsync(int accountId, DateTime dateFrom, DateTime dateTo)
        {
            var activityDataList = _repository.Execute(new GetActivityFromDatespan(accountId, dateFrom, dateTo));
            return activityDataList;
        }

        public async Task<ActivityData> ImportActivityAsync(ActivityData activityData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetActivityBySyncId(activityData.SyncId));

            if (entityToUpdate != null)
            {
                entityToUpdate.Duration = activityData.Duration;
                entityToUpdate.Created = activityData.Created;
                //todo save activity name

                return await _repository.UpdateAsync(entityToUpdate, saveChanges);
            }

            return await _repository.AddAsync(activityData, saveChanges);
        }

        public async Task<ActivityData> AddAsync(ActivityData activityData, bool saveChanges = true)
        {
            return await _repository.AddAsync(activityData, saveChanges);
        }

        public async Task<ActivityData> UpdateAsync(ActivityData activityData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetActivityById(activityData.Id));

            return await _repository.UpdateAsync(entityToUpdate, activityData, saveChanges);
        }

        public async Task ComputeNewActivityGoalAsync(int accountId)
        {

            try
            {
                if (accountId > 0)
                {
                    // Find last week activity log count (fetching last 30 days to optimize call)
                    var fromDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0).AddDays(-30);
                    var toDate = DateTime.Today.ToUniversalTime();
                    var activityDataList = await _repository.Execute(new GetAllActivitiesRangeByAccountId(accountId, fromDate, toDate));
                    if (activityDataList != null)
                    {
                        // If last week activity log count > 3 then calaulate
                        var lastWeek = DateTime.Today.ToUniversalTime().AddDays(-7);
                        var lastWeekLogsCount = activityDataList.Where(x => x.Created >= lastWeek).Count();
                        if (lastWeekLogsCount >= ActivityAlgorithmSettings.MaxActivityLogCount)
                        {
                            // Calulate Avg of last 30 days then increment or decrement new goal

                            TimeSpan average = activityDataList.Select(x => x.Duration).ToList().Mean();
                            if (average.TotalMinutes >= ActivityAlgorithmSettings.MinActivityMinutes)
                            {
                                // https://github.com/Prestine-Technologies-Pvt-Ltd/miboko/issues/300
                                // The possible activity duration goals are 20, 25, 30, 35 and 40 minutes only.
                                // When re-evaluating the goal, if the user's daily average activity duration for the past 30 days is above the user current goal then the user's goal is incremented by 5 minutes,
                                // to a maximum of 40 minutes. For example, if the user’s current goal is 25 minutes and they achieved a daily average of 35 minutes then their current goal is incremented to 30 minutes.
                                // When re-evaluating the goal, if the user's daily average activity duration for the past 30 days is below the user current goal then the user's goal is decremented by 5 minutes,
                                // to a minimum of 20 minutes.For example, if the user’s current goal is 30 minutes and they achieved a daily average of 15 minutes then their current goal is decremented to 25 minutes.

                                var currentGoalModel = await _repository.Execute(new GetPersonalGoalsByAccountId(accountId));
                                if (currentGoalModel != null)
                                {
                                    var currentGoal = currentGoalModel.ActivityDuration;
                                    if (average.TotalMinutes > currentGoal)
                                    {
                                        currentGoal += ActivityAlgorithmSettings.ActivityThreshold;
                                    }
                                    else
                                    {
                                        currentGoal -= ActivityAlgorithmSettings.ActivityThreshold;
                                    }
                                    if (currentGoal > ActivityAlgorithmSettings.MaxActivityMinutes) currentGoal = ActivityAlgorithmSettings.MaxActivityMinutes;
                                    if (currentGoal < ActivityAlgorithmSettings.ActivityThreshold) currentGoal = ActivityAlgorithmSettings.MinActivityMinutes;

                                    // Save updated Goal
                                    var newGoal = new PersonalGoal()
                                    {
                                        Id = currentGoalModel.Id,
                                        ActivityDuration = TimeSpan.FromMinutes(currentGoal).TotalMinutes,
                                        AccountId = currentGoalModel.AccountId,
                                        BodyFat = currentGoalModel.BodyFat,
                                        BodyMassIndex = currentGoalModel.BodyMassIndex,
                                        BreakfastAmount = currentGoalModel.BreakfastAmount,
                                        CalorieDrinkAlcoholConsumption = currentGoalModel.CalorieDrinkAlcoholConsumption,
                                        DinnerAmount = currentGoalModel.DinnerAmount,
                                        FastingDuration = currentGoalModel.FastingDuration,
                                        HealthySnackAmount = currentGoalModel.HealthySnackAmount,
                                        LunchAmount = currentGoalModel.LunchAmount,
                                        MeditationDuration = currentGoalModel.MeditationDuration,
                                        UnhealthySnackAmount = currentGoalModel.UnhealthySnackAmount,
                                        WaterConsumption = currentGoalModel.WaterConsumption,
                                        Weight = currentGoalModel.Weight
                                    };
                                    var res = await _repository.UpdateAsync(currentGoalModel, newGoal, true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}