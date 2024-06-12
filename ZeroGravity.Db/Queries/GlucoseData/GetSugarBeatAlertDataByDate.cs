using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models.SugarBeatData;
using ZeroGravity.Db.Repository;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Db.Queries.GlucoseData
{
    public class GetSugarBeatAlertDataByDate : IDbQuery<SugarBeatAlertData, ZeroGravityContext>
    {
        private readonly int _accountId;
        private readonly DateTime _time;
        private readonly string _transmitterId;
        private readonly AlertCode _code;

        public GetSugarBeatAlertDataByDate(int accountId, DateTime time, string transmitterId, AlertCode code)
        {
            _accountId = accountId;
            _time = time;
            _transmitterId = transmitterId;
            _code = code;
        }

        public async Task<SugarBeatAlertData> Execute(ZeroGravityContext dbContext)
        {
            // Check for same time periods
            SugarBeatAlertData alert = await dbContext.SugarBeatAlertDatas
                          .Where(x =>
                                 x.Created == _time &&
                                 x.TransmitterId == _transmitterId &&
                                 x.Code == _code &&
                                 x.AccountId == _accountId)
                          .FirstOrDefaultAsync();
            return alert;
        }
    }
}