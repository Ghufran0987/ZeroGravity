using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.Weight
{
    public class GetWeightTrackerDataById : IDbQuery<WeightTracker, ZeroGravityContext>
    {
        private readonly int _id;

        public GetWeightTrackerDataById(int id)
        {
            _id = id;
        }

        public async Task<WeightTracker> Execute(ZeroGravityContext dbContext)
        {
            var find = await dbContext.WeightTrackers.Include(x => x.Weights).FirstOrDefaultAsync(_ => _.Id == _id);
            dbContext.Entry(find).Reload();
            return find;
        }
    }
}