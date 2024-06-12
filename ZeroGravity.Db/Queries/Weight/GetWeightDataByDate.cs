using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.Weight
{
    public class GetWeightDataByDate : IDbQuery<WeightData, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly WeightData _weightData;

        public GetWeightDataByDate(int accountId, WeightData weight)
        {
            _accountId = accountId;
            _weightData = weight;
        }

        public async Task<WeightData> Execute(ZeroGravityContext dbContext)
        {
            var found = await dbContext.WeightDatas.Where(x =>
                        x.Created.Day == _weightData.Created.Day &&
                        x.Created.Month == _weightData.Created.Month &&
                        x.Created.Year == _weightData.Created.Year &&
                        x.WeightTrackerId == _weightData.WeightTrackerId &&
                        x.AccountId == _accountId).FirstOrDefaultAsync();
            return found;
        }
    }
}