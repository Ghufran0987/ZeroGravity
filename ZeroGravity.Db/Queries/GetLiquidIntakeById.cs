using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetLiquidIntakeById : IDbQuery<LiquidIntake, ZeroGravityContext>
    {
        private readonly long _id;

        public GetLiquidIntakeById(long id)
        {
            _id = id;
        }

        public async Task<LiquidIntake> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.LiquidIntakes.FirstOrDefaultAsync(_ =>
                _.Id == _id);
        }
    }
}