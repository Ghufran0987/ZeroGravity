using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatSessionDataById : IDbQuery<SugarBeatSessionData, ZeroGravityContext>
    {
        private readonly int _id;
        private readonly bool _includeGlucoseData;

        public GetSugarBeatSessionDataById(int id, bool includeGlucoseData = false)
        {
            _id = id;
            _includeGlucoseData = includeGlucoseData;
        }

        public async Task<SugarBeatSessionData> Execute(ZeroGravityContext dbContext)
        {
            if (_includeGlucoseData)
            {
                return await dbContext.SugarBeatSessions.Include(s => s.GlucoseDatas).FirstOrDefaultAsync(_ => _.Id == _id);
            }
            else
            {
                return await dbContext.SugarBeatSessions.FirstOrDefaultAsync(_ => _.Id == _id);
            }
        }
    }
}