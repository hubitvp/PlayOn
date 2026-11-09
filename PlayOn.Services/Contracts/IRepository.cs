namespace PlayOn.Services.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Query(bool asNoTracking = true);

        ValueTask<TEntity?> FindAsync(params object[] keyValues);

        Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);

        Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        
        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);
    }
}

