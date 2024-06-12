using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetMeditationByMeditationId : IDbQuery<MeditationData, ZeroGravityContext>
    {
        private readonly int _id;

        public GetMeditationByMeditationId(int id)
        {
            _id = id;
        }

        public async Task<MeditationData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.MeditationDatas.FirstOrDefaultAsync(_ => _.Id == _id);
        }
    }
}
