using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZeroGravity.Shared.Helper;

namespace ZeroGravity.Db.Repository
{
    public class Repository<TDbContext> : IRepository<TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly TDbContext _context;
        private readonly ILogger _logger;

        public Repository(TDbContext context, ILogger<Repository<TDbContext>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<T> AddAsync<T>(T entity, bool saveChanges = true, TDbContext context = null) where T : class
        {
            return await AddInternalAsync(entity, saveChanges, context ?? _context);
        }

        public async Task<TEntity> GetAsync<TEntity>(long id, TDbContext context = null) where TEntity : class
        {
            return await GetInternalAsync<TEntity>(id, context ?? _context);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(TDbContext context = null) where TEntity : class
        {
            return await GetAllInternalAsync<TEntity>(context ?? _context);
        }

        public async Task<T> UpdateAsync<T>(T entity, bool saveChanges = true, TDbContext context = null)
            where T : class
        {
            return await UpdateInternalAsync(entity, saveChanges, context ?? _context);
        }

        public async Task<T> UpdateAsync<T>(T entity, T unsavedItem, bool saveChanges = true, TDbContext context = null)
            where T : class
        {
            return await UpdateInternalAsync(entity, unsavedItem, saveChanges, context ?? _context);
        }

        public async Task DeleteAsync<T>(T entity, bool saveChanges = true, TDbContext context = null) where T : class
        {
            await DeleteInternalAsync(entity, saveChanges, context ?? _context);
        }

        public async Task<TOut> Execute<TOut>(IDbQuery<TOut, TDbContext> query, TDbContext context = null)
        {
            try
            {
                return await query.Execute(context ?? _context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:Execute<TOut> failed!");

                throw new DbException($"Repository:Execute<TOut>! \n {e.StackTrace}");
            }
        }

        public async Task Execute(IDbCommand<TDbContext> command, TDbContext context = null)
        {
            try
            {
                await command.Execute(context ?? _context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:Execute failed!");

                throw new DbException($"Repository:Execute! \n {e.StackTrace}");
            }
        }

        private async Task<T> AddInternalAsync<T>(T entity, bool saveChanges, TDbContext context) where T : class
        {
            try
            {
                var entry = await context.Set<T>().AddAsync(entity);

                if (saveChanges)
                {
                    await context.SaveChangesAsync();
                }

                return entry.Entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:AddInternalAsync<T> failed!");

                throw new DbException($"Repository:AddInternalAsync<T> failed! \n {e.StackTrace}");
            }
        }

        private async Task<TEntity> GetInternalAsync<TEntity>(long id, TDbContext context) where TEntity : class
        {
            try
            {
                return await context.Set<TEntity>().FindAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:GetInternalAsync<TEntity> failed!");

                throw new DbException($"Repository:GetInternalAsync<TEntity> failed! \n {e.StackTrace}");
            }
        }

        private async Task<IEnumerable<TEntity>> GetAllInternalAsync<TEntity>(TDbContext context) where TEntity : class
        {
            try
            {
                return await context.Set<TEntity>().ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:GetAllInternalAsync<TEntity> failed!");

                throw new DbException($"Repository:GetAllInternalAsync<TEntity> failed! \n {e.StackTrace}");
            }
        }

        private async Task<T> UpdateInternalAsync<T>(T entity, bool saveChanges, TDbContext context) where T : class
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                if (saveChanges)
                {
                    await context.SaveChangesAsync();
                }

                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:UpdateInternalAsync<T>  failed!");

                throw new DbException($"Repository:UpdateInternalAsync<T> failed! \n {e.StackTrace}");
            }
        }

        private async Task<T> UpdateInternalAsync<T>(T entity, T unsavedItem, bool saveChanges, TDbContext context) where T : class
        {
            try
            {
                context.Entry(entity).CurrentValues.SetValues(unsavedItem);

                // context.Entry(entity).Collections.
                context.Entry(entity).State = EntityState.Modified;

                if (saveChanges)
                {
                    await context.SaveChangesAsync();
                }

                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:UpdateInternalAsync<T>  failed!");

                throw new DbException($"Repository:UpdateInternalAsync<T> failed! \n {e.StackTrace}");
            }
        }

        private async Task DeleteInternalAsync<T>(T entity, bool saveChanges, TDbContext context) where T : class
        {
            try
            {
                context.Entry(entity).State = EntityState.Deleted;
                if (saveChanges)
                {
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:DeleteInternalAsync<T>  failed!");

                throw new DbException($"Repository:DeleteInternalAsync<T> failed! \n {e.StackTrace}");
            }
        }

        public async Task<IEnumerable<T>> UpdateBulkAsync<T>(IEnumerable<T> entities, bool saveChanges = true, TDbContext context = null) where T : class
        {
            try
            {
                context = context ?? _context;

                foreach (var entity in entities)
                {
                    context.Entry(entity).State = EntityState.Modified;
                }
                if (saveChanges)
                {
                    await context.SaveChangesAsync();
                }
                return entities;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:UpdateInternalAsync<T>  failed!");

                throw new DbException($"Repository:UpdateInternalAsync<T> failed! \n {e.StackTrace}");
            }
        }

        public async Task Save()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Repository:Save  failed!");
                throw new DbException($"Repository:Save failed! \n {e.StackTrace}");
            }
        }
    }
}