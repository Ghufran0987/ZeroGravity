using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetActivityFromDatespan : IDbQuery<List<ActivityData>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _initialDateTime;
        private readonly DateTime _targetDateTime;

        public GetActivityFromDatespan(long accountId, DateTime initialDateTime, DateTime targetDateTime)
        {
            _accountId = accountId;
            _targetDateTime = targetDateTime;
            _initialDateTime = initialDateTime;
        }

        public async Task<List<ActivityData>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.ActivityDatas.Where(_ =>
                _.AccountId == _accountId && _.Created.Date >= _initialDateTime.Date &&
                _.Created.Date <= _targetDateTime).ToListAsync();
        }
    }
}