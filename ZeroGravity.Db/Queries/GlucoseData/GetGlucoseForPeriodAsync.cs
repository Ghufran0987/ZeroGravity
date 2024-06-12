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
    public class GetGlucoseForPeriodAsync : IDbQuery<List<SugarBeatGlucoseData>, ZeroGravityContext>
    {
        private readonly int _accountid;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly bool _isGlucoseNull;

        public GetGlucoseForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool isGlucoseNull = false)
        {
            _accountid = accountId;
            _toDate = toDate;
            _fromDate = fromDate;
            _isGlucoseNull = isGlucoseNull;
        }

        public async Task<List<SugarBeatGlucoseData>> Execute(ZeroGravityContext dbContext)
        {
            var todateUTC = _toDate + QueryConstants.DayDuration;
            if (_isGlucoseNull)
            {
                return await dbContext.SugarBeatGlucoseDatas.Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid).OrderBy(x => x.Created).ToListAsync();
            }
            else
            {
                return await dbContext.SugarBeatGlucoseDatas.Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid && x.GlucoseValue != null).OrderBy(x => x.Created).ToListAsync();
            }
        }
    }
}