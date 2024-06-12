using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetMealTotalCountByAccountId : IDbQuery<int, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetMealTotalCountByAccountId(int accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<int> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.MealDatas.CountAsync(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc && _.Amount != FoodAmountType.None && _.Amount != FoodAmountType.Undefined);
        }
    }
}