using System.Linq.Expressions;

namespace PlayOn.Data.RepositoryContract
{
    /// <summary>
    /// Generic repository abstraction over an Entity Framework Core entity set.
    /// </summary>
    /// <typeparam name="TEntity">The entity type the repository works with.</typeparam>
    public interface IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Returns a queryable over all entities of type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="asNoTracking">
        /// When <c>true</c> (default), the returned entities are not tracked by the change tracker,
        /// which is faster for read-only scenarios. Pass <c>false</c> when the entities will be modified.
        /// </param>
        /// <returns>An <see cref="IQueryable{T}"/> that can be further composed with LINQ.</returns>
        IQueryable<TEntity> Query(bool asNoTracking = true);

        /// <summary>
        /// Finds an entity by its primary key value(s).
        /// The change tracker is checked first and the database is queried only if the entity is not already tracked.
        /// </summary>
        /// <param name="keyValues">The primary key value(s), in the order the key properties are defined.</param>
        /// <returns>The entity if found; otherwise <c>null</c>.</returns>
        ValueTask<TEntity?> FindAsync(params object[] keyValues);

        /// <summary>
        /// Begins tracking the given entity as added. It is inserted when <see cref="SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins tracking the given entities as added. They are inserted when <see cref="SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Begins tracking the given entity as modified. It is updated when <see cref="SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Begins tracking the given entity as deleted. It is removed when <see cref="SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Saves all pending changes in the underlying context to the database.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the original value of a property on a tracked entity.
        /// Typically used to supply a client-provided concurrency token (e.g. <c>RowVersion</c>)
        /// so that optimistic concurrency checks are performed against it.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="entity">The tracked entity.</param>
        /// <param name="propertyExpression">An expression selecting the property to set.</param>
        /// <param name="value">The original value to set.</param>
        void SetOriginalPropertyValue<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            TProperty value);

        /// <summary>
        /// Stops tracking all entities currently tracked by the underlying context.
        /// Useful for discarding unsaved changes, for example after a concurrency conflict.
        /// </summary>
        public void ClearChangeTracker();

        /// <summary>
        /// Executes the given operation inside a database transaction, using the context's execution strategy
        /// so the whole unit of work is retried as one if a transient failure occurs.
        /// The transaction is committed when the operation completes successfully.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the operation.</typeparam>
        /// <param name="isolationLevel">The isolation level of the transaction.</param>
        /// <param name="operation">The work to execute within the transaction.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>The result returned by <paramref name="operation"/>.</returns>
        Task<TResult> ExecuteInTransactionAsync<TResult>(
            System.Data.IsolationLevel isolationLevel,
            Func<CancellationToken, Task<TResult>> operation,
            CancellationToken cancellationToken = default);
    }
}

