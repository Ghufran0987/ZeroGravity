using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetActiveFastingDataByAccountIdAndDate : IDbQuery<IEnumerable<FastingData>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _dateTime;

        public GetActiveFastingDataByAccountIdAndDate(long accountId, DateTime dateTime)
        {
            _accountId = accountId;
            _dateTime = dateTime;
        }

        public async Task<IEnumerable<FastingData>> Execute(ZeroGravityContext dbContext)
        {
            var result = await dbContext.FastingDatas
            .Where(_ => _.AccountId == _accountId && (_.Created.Date == _dateTime.Date ||
        (_.End.Year == _dateTime.Year && _.End.Month == _dateTime.Month && _.End.Day == _dateTime.Day)))
            .OrderByDescending(x => x.End.Date).FirstOrDefaultAsync();

            if (result != null)
            {
                return new List<FastingData>() { result };
            }
            return new List<FastingData>();
        }
    }
}