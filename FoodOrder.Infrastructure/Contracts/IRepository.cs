using FoodOrder.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrder.Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        DbSet<TEntity> Entities { get; }
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }

        void Add(TEntity entity);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        TEntity GetById(params object[] ids);
        Task<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}