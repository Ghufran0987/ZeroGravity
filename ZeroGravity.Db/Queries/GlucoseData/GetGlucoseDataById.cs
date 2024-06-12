using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetGlucoseDataById : IDbQuery<Models.GlucoseData, ZeroGravityContext>
    {
        private readonly int _id;
        private readonly bool _getSugarBeatData;

        public GetGlucoseDataById(int id, bool getSugarBeatData = false)
        {
            _id = id;
            _getSugarBeatData = getSugarBeatData;
        }

        public async Task<Models.GlucoseData> Execute(ZeroGravityContext dbContext)
        {
            if (_getSugarBeatData)
            {
                return await dbContext.GlucoseDatas.Include(s => s.SugarBeatData).FirstOrDefaultAsync(_ => _.Id == _id);
            }

            return await dbContext.GlucoseDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}