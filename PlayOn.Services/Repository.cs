using Microsoft.EntityFrameworkCore;
using PlayOn.Data;
using PlayOn.Services.Contracts;
using System.Linq.Expressions;

namespace PlayOn.Services
{
    public sealed class Repository<TEntity>(PlayOnDbContext context)
        : IRepository<TEntity>
        where TEntity : class
    {
        public IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }


        public ValueTask<TEntity?> FindAsync(params object[] keyValues)
        {
            return context.Set<TEntity>().FindAsync(keyValues);
        }

        public Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            return context.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();
        }

        public Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            return context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }


        public void SetOriginalPropertyValue<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            TProperty value)
        {
            context.Entry(entity)
                .Property(propertyExpression)
                .OriginalValue = value;
        }
    }
}

