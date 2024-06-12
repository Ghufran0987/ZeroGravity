using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class IsSessionWarmedUp : IDbQuery<bool, ZeroGravityContext>
    {
        private readonly int _sessionId;
        private readonly bool _includeGlucoseData;

        public IsSessionWarmedUp(int sessionId)
        {
            _sessionId = sessionId;
        }

        public async Task<bool> Execute(ZeroGravityContext dbContext)
        {
            var res = await dbContext.SugarBeatGlucoseDatas.Where(x => x.IsSensorWarmedUp == true && x.SessionId == _sessionId).CountAsync();
            if (res > 0) return true;
            return false;
        }
    }
}