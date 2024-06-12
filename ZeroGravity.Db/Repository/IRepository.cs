using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZeroGravity.Db.Repository
{
    public interface IRepository<TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        Task<T> AddAsync<T>(T entity, bool saveChanges = true, TDbContext context = null) where T : class;

        Task<TEntity> GetAsync<TEntity>(long id, TDbContext context = null) where TEntity : class;

        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(TDbContext context = null) where TEntity : class;

        Task<T> UpdateAsync<T>(T entity, bool saveChanges = true, TDbContext context = null) where T : class;

        Task<IEnumerable<T>> UpdateBulkAsync<T>(IEnumerable<T> entities, bool saveChanges = true, TDbContext context = null) where T : class;

        Task<T> UpdateAsync<T>(T entity, T unsavedItem, bool saveChanges = true, TDbContext context = null) where T : class;

        Task DeleteAsync<T>(T entity, bool saveChanges = true, TDbContext context = null) where T : class;

        Task<TOut> Execute<TOut>(IDbQuery<TOut, TDbContext> query, TDbContext context = null);

        Task Execute(IDbCommand<TDbContext> command, TDbContext context = null);

        Task Save();
    }
}