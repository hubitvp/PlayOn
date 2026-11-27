using Microsoft.EntityFrameworkCore;
using PlayOn.Data.RepositoryContract;
using System.Linq.Expressions;

namespace PlayOn.Data.RepositoryImplementation
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

        public void ClearChangeTracker()
        {
            context.ChangeTracker.Clear();
        }

        public async Task<TResult> ExecuteInTransactionAsync<TResult>(
            System.Data.IsolationLevel isolationLevel,
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken = default)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async ct =>
            {
                await using var transaction = await context.Database
                    .BeginTransactionAsync(isolationLevel, ct);

                var result = await operation(ct);

                await transaction.CommitAsync(ct);

                return result;
            },
            cancellationToken);
        }


    }
}