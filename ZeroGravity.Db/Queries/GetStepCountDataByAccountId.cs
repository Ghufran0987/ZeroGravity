using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetStepCountDataByAccountId : IDbQuery<StepCountData, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetStepCountDataByAccountId(int accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<StepCountData> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.StepCountDatas.Where(_ => _.AccountId == _accountId && _.TargetDate >= _clientsBeginOfDayDateTimeInUtc && _.TargetDate <= clientsEndOfDayDateTimeInUtc).FirstOrDefaultAsync();
        }
    }
}