using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetLiquidIntakeNumberByAccountId : IDbQuery<int, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;
        private readonly LiquidType _liquidType;

        public GetLiquidIntakeNumberByAccountId(int accountId, DateTime dateTime, LiquidType liquidType)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
            _liquidType = liquidType;
        }

        public async Task<int> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.LiquidIntakes.CountAsync(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc && _.LiquidType == _liquidType);
        }
    }
}