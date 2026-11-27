using Microsoft.EntityFrameworkCore;
using PlayOn.Data.RepositoryContract;
using System.Linq.Expressions;

namespace PlayOn.Data.RepositoryImplementation
{
    /// <summary>
    /// Entity Framework Core implementation of <see cref="IRepository{TEntity}"/>
    /// that works on top of <see cref="PlayOnDbContext"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type the repository works with.</typeparam>
    /// <param name="context">The database context used to access the data.</param>
    public sealed class Repository<TEntity>(PlayOnDbContext context)
        : IRepository<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        /// <inheritdoc />
        public ValueTask<TEntity?> FindAsync(params object[] keyValues)
        {
            return context.Set<TEntity>().FindAsync(keyValues);
        }

        /// <inheritdoc />
        public Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            return context.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();
        }

        /// <inheritdoc />
        public Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            return context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc />
        public void Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        /// <inheritdoc />
        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        /// <inheritdoc />
        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public void SetOriginalPropertyValue<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            TProperty value)
        {
            context.Entry(entity)
                .Property(propertyExpression)
                .OriginalValue = value;
        }

        /// <inheritdoc />
        public void ClearChangeTracker()
        {
            context.ChangeTracker.Clear();
        }

        /// <inheritdoc />
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