using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetMealAmountByAccountId : IDbQuery<FoodAmountType, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly MealSlotType _mealSlotType;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetMealAmountByAccountId(int accountId, DateTime dateTime, MealSlotType mealSlotType)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
            _mealSlotType = mealSlotType;
        }

        public async Task<FoodAmountType> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;

            // temporäre Query, solange bis "Verhindern, dass mehr als ein Breakfast/Lunch/Dinner" implementiert ist
            var mealInfo = await dbContext.MealDatas.Where(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc && _.MealSlotType == _mealSlotType).ToListAsync();
            
            // sicherstellen, dass der letzt Eintrag in der DB für den User und dieses Datum gewählt wird
            var mealInfoOrdered = mealInfo.OrderBy(x => x.Id).ToList();
            var latestMealInfo = mealInfoOrdered.LastOrDefault();

            if (latestMealInfo != null)
            {
                return latestMealInfo.Amount;
            }

            //var mealInfo =  await dbContext.MealDatas.FirstOrDefaultAsync(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc && _.MealSlotType == _mealSlotType);
            //if (mealInfo != null)
            //{
            //    return mealInfo.Amount;
            //}

            return FoodAmountType.Undefined;
        }
    }
}
