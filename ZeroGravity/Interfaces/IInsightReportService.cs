using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroGravity.Db.Models;

namespace ZeroGravity.Interfaces
{
    public interface IInsightReportService
    {
        Task<List<InsightReportVideo>> GetInsightReportVideoBySessionIdAsync(int sessionId);
    }
}
