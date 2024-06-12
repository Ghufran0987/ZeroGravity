using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetLiquidIntakesByDateAndType : IDbQuery<List<LiquidIntake>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;
        private readonly LiquidType _type;

        public GetLiquidIntakesByDateAndType(long accountId, DateTime dateTime, LiquidType type)
        {
            _type = type;
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<List<LiquidIntake>> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.LiquidIntakes.Where(_ => _.AccountId == _accountId && _.LiquidType == _type && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc).ToListAsync();
        }
    }
}