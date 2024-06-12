using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatActiveSessionData : IDbQuery<SugarBeatSessionData, ZeroGravityContext>
    {
        private readonly int _accountid;
        private readonly DateTime _targetDate;
        private readonly bool _includeGlucoseData;

        public GetSugarBeatActiveSessionData(int accountId, DateTime targetDate, bool includeGlucoseData = false)
        {
            _accountid = accountId;
            _targetDate = targetDate;
            _includeGlucoseData = includeGlucoseData;
        }

        public async Task<SugarBeatSessionData> Execute(ZeroGravityContext dbContext)
        {
            // TODO Find Active Session from Given Time (Max 14 hours)
            // var fromDateUtc = _targetDate - QueryConstants.SensorPatchSessionDuration;
            if (_includeGlucoseData)
            {
                return await dbContext.SugarBeatSessions
                    .Include(x => x.GlucoseDatas)
                    .Where(x => x.Created <= _targetDate && x.EndTime >= _targetDate && x.AccountId == _accountid)
                    .OrderByDescending(x => x.Created)
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await dbContext.SugarBeatSessions
                    .Where(x => x.Created <= _targetDate && x.EndTime >= _targetDate && x.AccountId == _accountid)
                    .OrderByDescending(x => x.Created)
                    .FirstOrDefaultAsync();
            }
        }
    }
}