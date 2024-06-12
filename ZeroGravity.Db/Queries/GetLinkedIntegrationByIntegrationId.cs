using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetLinkedIntegrationByIntegrationId : IDbQuery<LinkedIntegration, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly int _integrationId;

        public GetLinkedIntegrationByIntegrationId(int accountId, int integrationId)
        {
            _accountId = accountId;
            _integrationId = integrationId;
        }

        public async Task<LinkedIntegration> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.LinkedIntegrations
                .FirstOrDefaultAsync(_ => _.AccountId == _accountId && _.IntegrationId == _integrationId);
        }
    }
}