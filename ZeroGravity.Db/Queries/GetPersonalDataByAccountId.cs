using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetPersonalDataByAccountId : IDbQuery<PersonalData, ZeroGravityContext>
    {
        private readonly long _accountId;

        public GetPersonalDataByAccountId(long accountId)
        {
            _accountId = accountId;
        }

        public async Task<PersonalData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.PersonalDatas.Include(x => x.QuestionAndAnswers).FirstOrDefaultAsync(_ => _.AccountId == _accountId);
        }
    }
}