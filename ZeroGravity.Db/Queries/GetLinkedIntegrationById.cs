using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetLinkedIntegrationById : IDbQuery<LinkedIntegration, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly int _integrationId;

        public GetLinkedIntegrationById(int accountId, int integrationId)
        {
            _integrationId = integrationId;
            _accountId = accountId;
        }

        public async Task<LinkedIntegration> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.LinkedIntegrations.FirstOrDefaultAsync(_ =>
                _.AccountId == _accountId && _.IntegrationId == _integrationId);
        }
    }
}