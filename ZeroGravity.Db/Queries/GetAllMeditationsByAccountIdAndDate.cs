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
    public class GetAllMeditationsByAccountIdAndDate : IDbQuery<List<MeditationData>, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _clientsBeginOfDayDateTimeInUtc;

        public GetAllMeditationsByAccountIdAndDate(int accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _clientsBeginOfDayDateTimeInUtc = dateTime;
        }

        public async Task<List<MeditationData>> Execute(ZeroGravityContext dbContext)
        {
            var clientsEndOfDayDateTimeInUtc = _clientsBeginOfDayDateTimeInUtc + QueryConstants.DayDuration;
            return await dbContext.MeditationDatas.Where(_ => _.AccountId == _accountId && _.Created >= _clientsBeginOfDayDateTimeInUtc && _.Created <= clientsEndOfDayDateTimeInUtc).ToListAsync();
        }
    }
}
