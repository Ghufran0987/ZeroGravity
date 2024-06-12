using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetWellbeingDataById : IDbQuery<WellbeingData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetWellbeingDataById(int id)
        {
            _id = id;
        }

        public async Task<WellbeingData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.WellbeingDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}