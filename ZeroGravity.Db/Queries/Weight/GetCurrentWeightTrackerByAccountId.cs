using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.Weight
{
    public class GetCurrentWeightTrackerByAccountId : IDbQuery<WeightTracker, ZeroGravityContext>
    {
        private readonly int _accountid;

        public GetCurrentWeightTrackerByAccountId(int accountId)
        {
            _accountid = accountId;
        }

        public async Task<WeightTracker> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.WeightTrackers
                .Where(x => x.AccountId == _accountid)
                .Include(x => x.Weights)
                .OrderByDescending(x => x.Created).FirstOrDefaultAsync();
        }
    }
}