using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatAlertDataById : IDbQuery<SugarBeatAlertData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetSugarBeatAlertDataById(int id)
        {
            _id = id;
        }

        public async Task<SugarBeatAlertData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.SugarBeatAlertDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}