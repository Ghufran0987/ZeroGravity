using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.Weight
{
    public class GetWeightTrackerByDateRange : IDbQuery<bool, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly WeightTracker _weightTracker;

        public GetWeightTrackerByDateRange(int accountId, WeightTracker weightTracker)
        {
            _accountId = accountId;
            _weightTracker = weightTracker;
        }

        public async Task<bool> Execute(ZeroGravityContext dbContext)
        {
            // Check for overlapping periods
            var found = await dbContext.WeightTrackers
                          .Where(x => !(x.Created > _weightTracker.Completed || x.Completed < _weightTracker.Created)
                          && x.AccountId == _accountId)
                          .CountAsync();
            if (found > 0) return true;
            return false;
        }
    }
}