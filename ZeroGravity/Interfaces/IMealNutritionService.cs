using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IMealNutritionService
    {
        Task<MealNutrition> GetByMealDataIdAsync(int mealDataId, bool includeMealComponentNutritionAndFoodSwaps);
    }
}
