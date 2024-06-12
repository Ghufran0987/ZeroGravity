using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetIntegrationByName : IDbQuery<IntegrationData, ZeroGravityContext>
    {
        private readonly string _name;


        public GetIntegrationByName(string name)
        {
            _name = name;
        }

        public async Task<IntegrationData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.IntegrationDatas.FirstOrDefaultAsync(_ => _.Name == _name);
        }
    }
}