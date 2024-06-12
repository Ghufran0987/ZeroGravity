using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class MealDataService : IMealDataService
    {
        private readonly ILogger<MealDataService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;
        private readonly ISugarBeatEatingSessionDataService _sugarBeatEatingSessionDataService;
        private readonly ISugarBeatSessionDataService _sugarBeatSessionDataService;

        public MealDataService(ILogger<MealDataService> logger, IRepository<ZeroGravityContext> repository, ISugarBeatEatingSessionDataService sugarBeatEatingSessionDataService, ISugarBeatSessionDataService sugarBeatSessionDataService)
        {
            _logger = logger;
            _repository = repository;
            _sugarBeatEatingSessionDataService = sugarBeatEatingSessionDataService;
            _sugarBeatSessionDataService = sugarBeatSessionDataService;
        }

        public Task<List<MealData>> GetByAccoundAndDateAsync(int accountId, DateTime targetDate)
        {
            var meals = _repository.Execute(new GetMealsByAccountId(accountId, targetDate));

            return meals;
        }

        public Task<List<MealData>> GetByAccountAndDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            return _repository.Execute(new GetMealFromDatespan(accountId, fromDate, toDate));
        }

        public Task<MealData> GetByIdAsync(int id, bool getIngredients = false)
        {
            var mealData = _repository.Execute(new GetMealById(id, getIngredients));

            return mealData;
        }

        public async Task<MealData> AddAsync(MealData mealData, bool saveChanges = true)
        {
            var result = await _repository.AddAsync(mealData, saveChanges);

            if (result != null)
            {
                // If we are adding a main meal while a sensor session is in progress then automate the creation of an eating session.
                var eatingSession = await CreateEatingSessionForMealData(mealData);
            }

            return result;
        }

        public async Task<MealData> UpdateAsync(MealData mealData, bool saveChanges = true)
        {
            var entityToUpdate = await _repository.Execute(new GetMealById(mealData.Id, true));
            var originalEntityCreatedDate = entityToUpdate.Created;
            entityToUpdate.Ingredients.Clear();
            foreach (var mealIngredientsBase in mealData.Ingredients)
            {
                entityToUpdate.Ingredients.Add(mealIngredientsBase);
            }

            entityToUpdate.Amount = mealData.Amount;
            entityToUpdate.Created = mealData.Created;
            entityToUpdate.Name = mealData.Name;

            var result = await _repository.UpdateAsync(entityToUpdate);

            // Check if there is an associated eating session and update it
            if (mealData.MealSlotType == MealSlotType.Breakfast || mealData.MealSlotType == MealSlotType.Lunch || mealData.MealSlotType == MealSlotType.Dinner)
            {
                var associatedEatingSession = await _sugarBeatEatingSessionDataService.GetSessionByDateAsync(mealData.AccountId, originalEntityCreatedDate);
                if (associatedEatingSession != null)
                {
                    associatedEatingSession.StartTime = mealData.Created;
                    associatedEatingSession.EndTime = mealData.Created.AddHours(GlucoseConstants.EatingSessionPeriod);
                    associatedEatingSession.MetabolicScore = 0;     // reset the metabolic score
                    associatedEatingSession.IsCompleted = false;    // reset this so that the metabolic score can get re-calculated
                    await _repository.UpdateAsync(associatedEatingSession);
                }
            }

            return result;

        }

        public async Task<List<MealData>> GetWithFilter(FilterMealDataDto dto)
        {
            var query = new GetMealsWithFilterQuery(dto);
            var meals = await _repository.Execute(query);
            return meals;
        }

        private async Task<SugarBeatEatingSession> CreateEatingSessionForMealData(MealData mealData)
        {
            // If we are adding a main meal while a sensor session is in progress then automate the creation of an eating session.

            if (mealData.MealSlotType == MealSlotType.Breakfast || mealData.MealSlotType == MealSlotType.Lunch || mealData.MealSlotType == MealSlotType.Dinner)
            {
                var sensorSessionsInRange = await _sugarBeatSessionDataService.GetSessionForPeriodAsync(mealData.AccountId, mealData.Created.AddHours(-GlucoseConstants.SensorExpiryPeriod), mealData.Created.AddHours(GlucoseConstants.SensorExpiryPeriod));

                if (sensorSessionsInRange != null && sensorSessionsInRange.Count > 0)
                {
                    // A sensor session is active during the time this meal was consumed

                    // Check to see if an eating session is already in progress
                    var eatingSessionsInRange = await _sugarBeatEatingSessionDataService.GetSessionForPeriodAsync(mealData.AccountId,
                        mealData.Created.AddHours(-GlucoseConstants.EatingSessionPeriod),
                        mealData.Created.AddHours(GlucoseConstants.EatingSessionPeriod));

                    if (eatingSessionsInRange == null || eatingSessionsInRange.Count == 0)
                    {
                        // An eating session is not active during the time this meal was consumed, so we can create one

                        var eatingSession = await _sugarBeatEatingSessionDataService.AddAsync(new SugarBeatEatingSession
                        {
                            AccountId = mealData.AccountId,
                            StartTime = mealData.Created,
                            EndTime = mealData.Created.AddHours(GlucoseConstants.EatingSessionPeriod),
                            IsCompleted = false
                            
                        });

                        return eatingSession;
                    }
                }
            }

            return null;
        }
    }
}