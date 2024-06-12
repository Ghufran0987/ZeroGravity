using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatEatingSessionForDate : IDbQuery<SugarBeatEatingSession, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _startDate;

        public GetSugarBeatEatingSessionForDate(int accountId, DateTime startDate)
        {
            _accountId = accountId;
            _startDate = startDate;
        }

        public async Task<SugarBeatEatingSession> Execute(ZeroGravityContext dbContext)
        {
            var find = await dbContext.SugarBeatEatingSessions.Where(x => x.StartTime == _startDate && x.AccountId == _accountId).OrderByDescending(x => x.StartTime).FirstOrDefaultAsync();
            return find;
        }
    }
}