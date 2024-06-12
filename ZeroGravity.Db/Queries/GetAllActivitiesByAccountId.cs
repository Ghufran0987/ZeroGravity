using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetAllActivitiesByAccountId : IDbQuery<List<ActivityData>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetAllActivitiesByAccountId(int accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<List<ActivityData>> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            var result = await dbContext.ActivityDatas.Where(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc).ToListAsync();

            return result;
        }
    }

    public class GetAllActivitiesRangeByAccountId : IDbQuery<List<ActivityData>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _fromDateTimeInUtc;
        private readonly DateTime _toDateTimeInUtc;

        public GetAllActivitiesRangeByAccountId(int accountId, DateTime fromDateTime, DateTime toDateTime)
        {
            _accountId = accountId;
            _fromDateTimeInUtc = fromDateTime;
            _toDateTimeInUtc = toDateTime;
        }

        public async Task<List<ActivityData>> Execute(ZeroGravityContext dbContext)
        {
            var result = await dbContext.ActivityDatas.Where(_ => _.AccountId == _accountId && _.Created >= _fromDateTimeInUtc && _.Created <= _toDateTimeInUtc).ToListAsync();
            return result;
        }
    }
}