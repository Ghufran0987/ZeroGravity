using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetMealNumberByAccountId : IDbQuery<int, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly MealSlotType _mealSlotType;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetMealNumberByAccountId(int accountId, DateTime dateTime, MealSlotType mealSlotType)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
            _mealSlotType = mealSlotType;
        }

        public async Task<int> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.MealDatas.CountAsync(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc && _.MealSlotType == _mealSlotType);
        }
    }
}