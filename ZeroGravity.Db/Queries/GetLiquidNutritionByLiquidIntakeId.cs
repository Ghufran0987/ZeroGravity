using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetLiquidNutritionByLiquidIntakeId : IDbQuery<LiquidNutrition, ZeroGravityContext>
    {
        private readonly int _liquidIntakeId;
        private readonly bool _includeLiquidComponentNutrition;

        public GetLiquidNutritionByLiquidIntakeId(int liquidIntakeId, bool includeLiquidComponentNutrition = false)
        {
            _liquidIntakeId = liquidIntakeId;
            _includeLiquidComponentNutrition = includeLiquidComponentNutrition;
        }

        public async Task<LiquidNutrition> Execute(ZeroGravityContext dbContext)
        {

            if (_includeLiquidComponentNutrition)
            {
                return await dbContext.LiquidNutritions
                    .Include(x => x.LiquidComponentNutrition)
                    .FirstOrDefaultAsync(_ => _.LiquidIntakeId == _liquidIntakeId);
            }
            else
            {
                return await dbContext.LiquidNutritions
                    .FirstOrDefaultAsync(_ => _.LiquidIntakeId == _liquidIntakeId);

            }
        }
    }
}