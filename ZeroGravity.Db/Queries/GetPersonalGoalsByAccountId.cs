using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetPersonalGoalsByAccountId : IDbQuery<PersonalGoal, ZeroGravityContext>
    {
        private readonly long _accountId;

        public GetPersonalGoalsByAccountId(long accountId)
        {
            _accountId = accountId;
        }

        public async Task<PersonalGoal> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.PersonalGoals.FirstOrDefaultAsync(_ => _.AccountId == _accountId);
        }
    }
}