using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Interfaces
{
    public interface IMealDataService
    {
        Task<List<MealData>> GetByAccoundAndDateAsync(int accountId, DateTime targetDate);
        Task<List<MealData>> GetByAccountAndDateRangeAsync(int accountId, DateTime fromDate, DateTime toDate);
        Task<MealData> GetByIdAsync(int id, bool getIngredients);
        Task<MealData> AddAsync(MealData mealData, bool saveChanges = true);
        Task<MealData> UpdateAsync(MealData mealData, bool saveChanges = true);

        Task<List<MealData>> GetWithFilter(FilterMealDataDto dto);
    }
}
