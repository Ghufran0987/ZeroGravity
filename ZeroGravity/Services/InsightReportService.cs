using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Models;
using ZeroGravity.Db.Queries;
using ZeroGravity.Db.Repository;
using ZeroGravity.Interfaces;

namespace ZeroGravity.Services
{
    public class InsightReportService : IInsightReportService
    {
        private readonly ILogger<InsightReportService> _logger;
        private readonly IRepository<ZeroGravityContext> _repository;

        public InsightReportService(ILogger<InsightReportService> logger, IRepository<ZeroGravityContext> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public  Task<List<InsightReportVideo>> GetInsightReportVideoBySessionIdAsync(int sessionId)
        {
            var insightReportVideos = _repository.Execute(new GetInsightReportVideoBySessionId(sessionId));

            return insightReportVideos;
        }
    }
}