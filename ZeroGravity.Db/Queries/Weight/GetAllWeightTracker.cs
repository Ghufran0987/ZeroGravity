using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.Weight
{
    public class GetAllWeightTracker : IDbQuery<List<WeightTracker>, ZeroGravityContext>
    {
        private readonly int _accountid;

        public GetAllWeightTracker(int accountId)
        {
            _accountid = accountId;
        }

        public async Task<List<WeightTracker>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.WeightTrackers.Where(x => x.AccountId == _accountid).OrderByDescending(x => x.Created).ToListAsync();
        }
    }
}