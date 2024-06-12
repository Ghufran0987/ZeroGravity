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
    public class GetFastingDataByAccountIdAndDateRange : IDbQuery<List<FastingData>, ZeroGravityContext>
    {
        private readonly long _accountId;
        private readonly DateTime _dateStart;
        private readonly DateTime _dateEnd;

        public GetFastingDataByAccountIdAndDateRange(long accountId, DateTime dateStart, DateTime dateEnd)
        {
            _accountId = accountId;
            _dateStart = dateStart;
            _dateEnd = dateEnd;
        }

        public async Task<List<FastingData>> Execute(ZeroGravityContext dbContext)
        {
            // Fasting data that starts between the given date range
            return await dbContext.FastingDatas.Where(f =>
                f.AccountId == _accountId
                && f.Start.Date >= _dateStart.Date 
                && f.Start.Date <= _dateEnd.Date
            ).OrderByDescending(x => x.Created).ToListAsync();
        }
    }
}