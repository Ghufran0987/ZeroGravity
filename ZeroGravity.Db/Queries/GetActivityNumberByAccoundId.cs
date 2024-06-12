using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.Constants;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries
{
    public class GetActivityNumberByAccoundId : IDbQuery<int, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly ActivityType _activityType;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetActivityNumberByAccoundId(int accountId, DateTime dateTime, ActivityType activityType)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
            _activityType = activityType;
        }

        public async Task<int> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            var result = await dbContext.ActivityDatas.CountAsync(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc && _.ActivityType == _activityType);
            
            return result;
        }
    }
}