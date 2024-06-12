using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetGlucoseDatasByAccountIdAndDateForSugarBeat : IDbQuery<List<Models.GlucoseData>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetGlucoseDatasByAccountIdAndDateForSugarBeat(int accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<List<Models.GlucoseData>> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.GlucoseDatas.Where(_ => _.AccountId == _accountId && _.Date >= _clientsBeginOfDayDateTimeInUtc && _.Date <= clientsEndOfDayDateTimeInUtc).ToListAsync();
        }
    }
}
