using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using StoredProcedureEFCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Db.Queries
{
    public class GetProgressByAccountIdForDay : IDbQuery<List<ProgressSummaryDto>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;
        private readonly DateTime _clientsEndOfDayDateTimeInUtc;

        public GetProgressByAccountIdForDay(int accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
            _clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
        }

        public async Task<List<ProgressSummaryDto>> Execute(ZeroGravityContext dbContext)
        {
            try
            {
                List<ProgressSummaryDto> result = null;

                dbContext.LoadStoredProc("dbo.GetProgressByDay")
                           .AddParam("@fromDate", _clientsBeginOfDayDateTimeInUtc)
                           .AddParam("@toDate", _clientsEndOfDayDateTimeInUtc)
                           .AddParam("@accountId", _accountId)
                           .Exec(r => result = r.ToList<ProgressSummaryDto>());

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in GetProgressByAccountIdForDay Execute: " + ex.Message + "Stack trace: " + ex.StackTrace);
                Debug.WriteLine("Exception in GetProgressByAccountIdForDay Params: acount id " +_accountId  + "ToDate: " + _clientsEndOfDayDateTimeInUtc + "FromDate: "+ _clientsBeginOfDayDateTimeInUtc);
                return null;
            }
        }
    }
}