using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatEatingSessionForPeriod : IDbQuery<List<SugarBeatEatingSession>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;

        public GetSugarBeatEatingSessionForPeriod(int accountId, DateTime fromDate, DateTime toDate)
        {
            _accountId = accountId;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public async Task<List<SugarBeatEatingSession>> Execute(ZeroGravityContext dbContext)
        {
            //var fromDate = new DateTime(_fromDate.Year, _fromDate.Month, _fromDate.Day, 0, 0, 0); 
            //var todateUTC = new DateTime(_toDate.Year, _toDate.Month, _toDate.Day, 23, 59,59);
            var find = await dbContext.SugarBeatEatingSessions.Where(x => x.StartTime >= _fromDate && x.EndTime <= _toDate && x.AccountId == _accountId).OrderByDescending(x=> x.StartTime).ToListAsync();
            return find;
        }
    }
}