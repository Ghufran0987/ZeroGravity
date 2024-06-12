using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetFastingDataByAccountIdAndDate : IDbQuery<FastingData, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _dateTime;

        public GetFastingDataByAccountIdAndDate(long accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _dateTime = dateTime;
        }

        public async Task<FastingData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.FastingDatas
                .Where(_ => _.AccountId == _accountId && (_.Created.Date == _dateTime.Date ||
            (_.End.Year == _dateTime.Year && _.End.Month == _dateTime.Month && _.End.Day == _dateTime.Day)))
                .OrderByDescending(x => x.End.Date).FirstOrDefaultAsync();
        }
    }
}