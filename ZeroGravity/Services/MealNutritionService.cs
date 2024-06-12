using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class MealNutritionService : IMealNutritionService
    {
        private readonly ILogger<MealNutritionService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public MealNutritionService(ILogger<MealNutritionService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public Task<MealNutrition> GetByMealDataIdAsync(int mealDataId, bool includeMealComponentNutritionAndFoodSwaps = false)
        {
            var mealNutrition = _repository.Execute(new GetMealNutritionByMealDataId(mealDataId, includeMealComponentNutritionAndFoodSwaps));

            return mealNutrition;
        }
    }
}