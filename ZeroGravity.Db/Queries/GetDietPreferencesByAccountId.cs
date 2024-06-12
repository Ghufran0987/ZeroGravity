using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetDietPreferencesByAccountId : IDbQuery<DietPreference, ZeroGravityContext>
    {
        private readonly int _accountId;

        public GetDietPreferencesByAccountId(int accountId)
        {
            _accountId = accountId;
        }

        public async Task<DietPreference> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.DietPreferences.FirstOrDefaultAsync(_ => _.AccountId == _accountId);
        }
    }
}
