using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetLinkedIntegrationsByAccountId : IDbQuery<List<LinkedIntegration>, ZeroGravityContext>
    {
        private readonly int _accountId;

        public GetLinkedIntegrationsByAccountId(int accountId)
        {
            _accountId = accountId;
        }

        public async Task<List<LinkedIntegration>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.LinkedIntegrations
                .Where(_ => _.AccountId == _accountId).ToListAsync();
        }
    }
}