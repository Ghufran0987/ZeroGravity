using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetActivityBySyncId : IDbQuery<ActivityData, ZeroGravityContext>
    {
        private readonly int _syncId;

        public GetActivityBySyncId(int syncId)
        {
            _syncId = syncId;
        }

        public async Task<ActivityData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.ActivityDatas.FirstOrDefaultAsync(_ => _.SyncId == _syncId);
        }
    }
}