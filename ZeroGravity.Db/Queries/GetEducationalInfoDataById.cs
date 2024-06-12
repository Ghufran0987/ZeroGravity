using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetEducationalInfoDataById : IDbQuery<EducationalInfoData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetEducationalInfoDataById(int id)
        {
            _id = id;
        }

        public async Task<EducationalInfoData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.EducationalInfos.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}