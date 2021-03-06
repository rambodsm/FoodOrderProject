using FoodOrder.Common.Utilities;
using FoodOrder.Domain.Common;
using FoodOrder.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly ApplicationDbContext DbContext;
        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>();
        }

        #region Async Method
        public virtual async Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {

            return await Entities.FindAsync(ids, cancellationToken);
        }
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            Assert.NotNull(entity, nameof(entity));
            await Entities.AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            Assert.NotNull(entities, nameof(entities));
            await Entities.AddRangeAsync(entities, cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.UpdateRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
        }
        #endregion

        #region Sync Methods
        public virtual TEntity GetById(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public virtual void Add(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.AddRange(entities);
        }
        #endregion
    }
}
