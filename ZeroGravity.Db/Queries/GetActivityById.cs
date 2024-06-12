using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetActivityById : IDbQuery<ActivityData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetActivityById(int id)
        {
            _id = id;
        }

        public async Task<ActivityData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.ActivityDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}