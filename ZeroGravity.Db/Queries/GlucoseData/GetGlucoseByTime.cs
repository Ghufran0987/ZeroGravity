using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetGlucoseByTime : IDbQuery<SugarBeatGlucoseData, ZeroGravityContext>
    {
        private readonly int _accountid;
        private readonly DateTime _time;

        public GetGlucoseByTime(int accountId, DateTime dateTime)
        {
            _accountid = accountId;
            _time = dateTime;
        }

        public async Task<SugarBeatGlucoseData> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.SugarBeatGlucoseDatas.Where(x => x.Created == _time && x.AccountId == _accountid).OrderBy(x => x.Created).FirstOrDefaultAsync();
        }
    }
}