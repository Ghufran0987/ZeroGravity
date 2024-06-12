using System.Threading.Tasks;

namespace ZeroGravity.Db.Repository
{
    public interface IDbCommand<in TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        Task Execute(TDbContext dbContext);
    }
}