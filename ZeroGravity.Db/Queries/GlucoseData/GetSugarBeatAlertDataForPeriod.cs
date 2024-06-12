using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatAlertDataForPeriod : IDbQuery<List<SugarBeatAlertData>, ZeroGravityContext>
    {
        private readonly int _accountid;
        private readonly DateTime _fromDate;
        private readonly DateTime _toDate;
        private readonly AlertCode? _alertCode;
        private readonly CRCCodes? _criticalCode;

        public GetSugarBeatAlertDataForPeriod(int accountId, DateTime fromDate, DateTime toDate, AlertCode? alertCode, CRCCodes? criticalCode)
        {
            _accountid = accountId;
            _toDate = toDate;
            _fromDate = fromDate;
            _criticalCode = criticalCode;
            _alertCode = alertCode;
        }

        public async Task<List<SugarBeatAlertData>> Execute(ZeroGravityContext dbContext)
        {
            var todateUTC = _toDate + QueryConstants.DayDuration;

            if (_alertCode != null && _criticalCode != null)
            {
                return await dbContext.SugarBeatAlertDatas
                    .Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid && x.Code == _alertCode && x.CriticalCode == _criticalCode)
                    .ToListAsync();
            }
            else if (_alertCode == null && _criticalCode != null)
            {
                return await dbContext.SugarBeatAlertDatas
                    .Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid && x.CriticalCode == _criticalCode)
                    .ToListAsync();
            }
            else if (_alertCode != null && _criticalCode == null)
            {
                return await dbContext.SugarBeatAlertDatas
                    .Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid && x.Code == _alertCode)
                    .ToListAsync();
            }
            else
            {
                return await dbContext.SugarBeatAlertDatas
                    .Where(x => x.Created >= _fromDate && x.Created <= todateUTC && x.AccountId == _accountid)
                    .ToListAsync();
            }
        }
    }
}