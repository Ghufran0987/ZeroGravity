using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetMedicalConditionByAccountId : IDbQuery<MedicalCondition, ZeroGravityContext>
    {
        private readonly long _accountId;

        public GetMedicalConditionByAccountId(long accountId)
        {
            _accountId = accountId;
        }

        public async Task<MedicalCondition> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.MedicalConditions.FirstOrDefaultAsync(_ => _.AccountId == _accountId);
        }
    }
}