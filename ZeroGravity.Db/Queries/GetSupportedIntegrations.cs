using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetSupportedIntegrations : IDbQuery<List<IntegrationData>, ZeroGravityContext>
    {
        public async Task<List<IntegrationData>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.IntegrationDatas.ToListAsync();
        }
    }
}