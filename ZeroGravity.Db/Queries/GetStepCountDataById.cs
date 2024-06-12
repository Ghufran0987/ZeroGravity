using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetStepCountDataById : IDbQuery<StepCountData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetStepCountDataById(int id)
        {
            _id = id;
        }

        public async Task<StepCountData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.StepCountDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}