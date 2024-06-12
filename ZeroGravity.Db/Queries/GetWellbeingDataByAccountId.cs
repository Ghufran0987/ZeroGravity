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
    public class GetWellbeingDataByAccountId : IDbQuery<WellbeingData, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetWellbeingDataByAccountId(long accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<WellbeingData> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.WellbeingDatas.Where(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc).OrderByDescending(x => x.Created).FirstOrDefaultAsync();
        }
    }

    public class GetAllWellbeingDataByAccountId : IDbQuery<List<WellbeingData>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetAllWellbeingDataByAccountId(long accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<List<WellbeingData>> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.WellbeingDatas.Where(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc).ToListAsync();
        }
    }

    public class GetAllWellbeingDataByAccountIdAndDateRange : IDbQuery<List<WellbeingData>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;

        public GetAllWellbeingDataByAccountIdAndDateRange(long accountId, DateTime dateFrom, DateTime dateTo)
        {
            _accountId = accountId;
            _dateFrom = dateFrom;
            _dateTo = dateTo;
        }

        public async Task<List<WellbeingData>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.WellbeingDatas.Where(_ => _.AccountId == _accountId && _.Created >= _dateFrom && _.Created <= _dateTo).ToListAsync();
        }
    }
}