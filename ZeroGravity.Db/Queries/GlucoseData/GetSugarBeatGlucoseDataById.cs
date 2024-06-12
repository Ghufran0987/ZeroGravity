using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatGlucoseDataById : IDbQuery<SugarBeatGlucoseData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetSugarBeatGlucoseDataById(int id)
        {
            _id = id;
        }

        public async Task<Models.SugarBeatData.SugarBeatGlucoseData> Execute(ZeroGravityContext dbContext)
        {
            var find = await dbContext.SugarBeatGlucoseDatas.FirstOrDefaultAsync(_ => _.Id == _id);

            dbContext.Entry(find).Reload();
            return find;
        }
    }
}