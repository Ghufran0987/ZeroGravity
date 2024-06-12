using System.Threading.Tasks;

namespace ZeroGravity.Db.Repository
{
    public interface IDbQuery<TOut, in TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        Task<TOut> Execute(TDbContext dbContext);
    }
}