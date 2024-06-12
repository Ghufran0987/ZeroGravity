using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetAllGlucoseForSessionId : IDbQuery<List<SugarBeatGlucoseData>, ZeroGravityContext>
    {
        private readonly int _accountid;
        private readonly int _sessionid;
        private readonly bool _includeGlucoseNull;

        public GetAllGlucoseForSessionId(int accountId, int sessionId, bool includeGlucoseNull = false)
        {
            _accountid = accountId;
            _sessionid = sessionId;
            _includeGlucoseNull = includeGlucoseNull;
        }

        public async Task<List<SugarBeatGlucoseData>> Execute(ZeroGravityContext dbContext)
        {
            if (_includeGlucoseNull)
            {
                return await dbContext.SugarBeatGlucoseDatas.Where(x => x.SessionId == _sessionid && x.AccountId == _accountid).OrderBy(x => x.Created).ToListAsync();
            }
            else
            {
                return await dbContext.SugarBeatGlucoseDatas.Where(x => x.SessionId == _sessionid && x.AccountId == _accountid && x.GlucoseValue != null).OrderBy(x => x.Created).ToListAsync();
            }
        }
    }
}