using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Db.Queries
{
    public class GetMealsWithFilterQuery : IDbQuery<List<MealData>, ZeroGravityContext>
    {
        private readonly FilterMealDataDto _dto;

        public GetMealsWithFilterQuery(FilterMealDataDto dto)
        {
            _dto = dto;
        }

        public async Task<List<MealData>> Execute(ZeroGravityContext dbContext)
        {
            IQueryable<MealData> queryable = dbContext.MealDatas;
            if (_dto == null) return await queryable.ToListAsync();

            queryable = queryable.Where(_ => _.AccountId == _dto.AccountId);

            if (_dto.WithIngredients) queryable = queryable.Include(_ => _.Ingredients);

            if (_dto.SlotType.HasValue) queryable = queryable.Where(_ => _.MealSlotType == _dto.SlotType.Value);

            if (_dto.FoodAmountType.HasValue)
            {
                queryable = queryable.Where(_ => _.Amount == _dto.FoodAmountType.Value);
            }
            //queryable = _dto.FoodAmountType.HasValue
            //    ? queryable.Where(_ => _.Amount == _dto.FoodAmountType.Value)
            //    : queryable.Where(_ => _.Amount != FoodAmountType.None);

            if (_dto.DateTime.HasValue)
            {
                var clientsEndOfDayDateTimeInUtc = _dto.DateTime.Value + QueryConstants.DayDuration;
                queryable = queryable.Where(_ =>
                    _.Created >= _dto.DateTime.Value && _.Created <= clientsEndOfDayDateTimeInUtc);
            }

            return await queryable.ToListAsync();
        }
    }
}