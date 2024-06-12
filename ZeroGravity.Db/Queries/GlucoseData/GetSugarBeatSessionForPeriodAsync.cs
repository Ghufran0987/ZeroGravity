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
    public class GetSugarBeatSessionForPeriodAsync : IDbQuery<List<SugarBeatSessionData>, ZeroGravityContext>
    {
        private const int maxHoursOfGlucoseData = 14;

        private readonly int _accountid;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly bool _includeGlucoseData;

        public GetSugarBeatSessionForPeriodAsync(int accountId, DateTime fromDate, DateTime toDate, bool includeGlucoseData = false)
        {
            _accountid = accountId;
            _toDate = toDate;
            _fromDate = fromDate;
            _includeGlucoseData = includeGlucoseData;
        }

        public async Task<List<SugarBeatSessionData>> Execute(ZeroGravityContext dbContext)
        {
            var todateUTC = _toDate + QueryConstants.DayDuration;
            if (_includeGlucoseData)
            {
                // ensure only a maximum of 14 hours worth of glucose data is returned.
                return await dbContext.SugarBeatSessions
                    .Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid)
                    .Select(x => new SugarBeatSessionData(x.Id, x.Created, x.AccountId, x.TransmitterId, x.BatteryVoltage, x.FirmwareVersion, x.EndTime, x.StartAlertId, x.StartAlert, x.EndAlertId, x.EndAlert, x.GlucoseDatas.Where(g => g.Created <= x.Created.AddHours(maxHoursOfGlucoseData)).ToList()))
                    .ToListAsync();
            }
            else
            {
                return await dbContext.SugarBeatSessions
                    .Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid)
                    .ToListAsync();
            }
        }
    }
}