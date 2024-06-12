using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatSettingDataByAccountId : IDbQuery<SugarBeatSettingData, ZeroGravityContext>
    {
        private readonly int _accountId;

        public GetSugarBeatSettingDataByAccountId(int accountId)
        {
            _accountId = accountId;
        }

        public async Task<SugarBeatSettingData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.SugarBeatSettings.FirstOrDefaultAsync(_ => _.AccountId == _accountId);
        }
    }
}