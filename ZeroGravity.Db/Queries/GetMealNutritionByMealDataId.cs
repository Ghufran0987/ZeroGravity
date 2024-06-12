using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetMealNutritionByMealDataId : IDbQuery<MealNutrition, ZeroGravityContext>
    {
        private readonly int _mealDataId;
        private readonly bool _includeMealComponentNutritionAndFoodSwaps;

        public GetMealNutritionByMealDataId(int mealDataId, bool includeMealComponentNutritionAndFoodSwaps = false)
        {
            _mealDataId = mealDataId;
            _includeMealComponentNutritionAndFoodSwaps = includeMealComponentNutritionAndFoodSwaps;
        }

        public async Task<MealNutrition> Execute(ZeroGravityContext dbContext)
        {

            if (_includeMealComponentNutritionAndFoodSwaps)
            {
                return await dbContext.MealNutritions
                    .Include(x => x.MealComponentNutrition)
                    .Include(x => x.MealFoodSwaps)
                    .FirstOrDefaultAsync(_ => _.MealDataId == _mealDataId);
            }
            else
            {
                return await dbContext.MealNutritions
                    .FirstOrDefaultAsync(_ => _.MealDataId == _mealDataId);

            }
        }
    }
}