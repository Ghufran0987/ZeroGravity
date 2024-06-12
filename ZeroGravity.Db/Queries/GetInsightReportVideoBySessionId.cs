using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Repository;

namespace ZeroGravity.Db.Queries
{
    public class GetInsightReportVideoBySessionId : IDbQuery<List<InsightReportVideo>, ZeroGravityContext>
    {
        private readonly int _sessionId;

        public GetInsightReportVideoBySessionId(int sessionId)
        {
            _sessionId = sessionId;
        }

        public async Task<List<InsightReportVideo>> Execute(ZeroGravityContext dbContext)
        {
            return await dbContext.InsightReportVideos.Where(_ =>
                _.SessionId == _sessionId).ToListAsync();
        }
    }
}