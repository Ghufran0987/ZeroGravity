using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetFastingSettingsByAccountId : IDbQuery<FastingSetting, ZeroGravityContext>
    {
        private readonly long _accountId;

        public GetFastingSettingsByAccountId(long accountId)
        {
            _accountId = accountId;
        }

        public async Task<FastingSetting> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.FastingSettings.FirstOrDefaultAsync(_ => _.AccountId == _accountId);
        }
    }
}