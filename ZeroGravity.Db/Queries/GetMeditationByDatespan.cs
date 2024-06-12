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
    public class GetMeditationByDatespan : IDbQuery<List<MeditationData>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _initialDateTime;
        private readonly DateTime _targetDateTime;

        public GetMeditationByDatespan(long accountId, DateTime initialDateTime, DateTime targetDateTime)
        {
            _accountId = accountId;
            _targetDateTime = targetDateTime;
            _initialDateTime = initialDateTime;
        }

        public async Task<List<MeditationData>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.MeditationDatas.Where(_ =>
                _.AccountId == _accountId && _.Created.Date >= _initialDateTime.Date &&
                _.Created.Date <= _targetDateTime).ToListAsync();
        }
    }
}