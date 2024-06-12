using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetRandomByCategory : IDbQuery<EducationalInfoData, ZeroGravityContext>
    {
        private readonly string _category;

        public GetRandomByCategory(string category)
        {
            _category = category;
        }

        public async Task<EducationalInfoData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.EducationalInfos
                .Where(x => x.Category.ToLower() == _category.ToLower())
                .OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync();
        }
    }
}