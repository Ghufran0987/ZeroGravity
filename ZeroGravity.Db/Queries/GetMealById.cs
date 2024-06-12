using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetMealById : IDbQuery<MealData, ZeroGravityContext>
    {
        private readonly int _id;
        private readonly bool _getIngredients;

        public GetMealById(int id, bool getIngredients = false)
        {
            _id = id;
            _getIngredients = getIngredients;
        }

        public async Task<MealData> Execute(ZeroGravityContext dbContext)
        {
            if (_getIngredients)
            {
                return await dbContext.MealDatas.Include(b => b.Ingredients).FirstOrDefaultAsync(_ => _.Id == _id);
            }

            return await dbContext.MealDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}