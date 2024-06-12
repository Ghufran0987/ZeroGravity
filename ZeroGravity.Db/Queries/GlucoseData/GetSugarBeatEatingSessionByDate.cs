using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatEatingSessionByDate : IDbQuery<bool, ZeroGravityContext>
    {
        private readonly SugarBeatEatingSession _sessionDto;

        public GetSugarBeatEatingSessionByDate(SugarBeatEatingSession sessionDto)
        {
            _sessionDto = sessionDto;
        }

        public async Task<bool> Execute(ZeroGravityContext dbContext)
        {
            // Check for overlapping periods
            var found = await dbContext.SugarBeatEatingSessions
                          .Where(x => !(x.StartTime > _sessionDto.EndTime || x.EndTime < _sessionDto.StartTime)
                          && x.AccountId == _sessionDto.AccountId)
                          .CountAsync();
            if (found > 0) return true;
            return false;
        }
    }
}