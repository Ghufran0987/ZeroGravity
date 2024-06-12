using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatEatingSessionById : IDbQuery<SugarBeatEatingSession, ZeroGravityContext>
    {
        private readonly int _id;

        public GetSugarBeatEatingSessionById(int id)
        {
            _id = id;
        }

        public async Task<SugarBeatEatingSession> Execute(ZeroGravityContext dbContext)
        {
            var find = await dbContext.SugarBeatEatingSessions.FirstOrDefaultAsync(_ => _.Id == _id);
            dbContext.Entry(find).Reload();
            return find;
        }
    }
}