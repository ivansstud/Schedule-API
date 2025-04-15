namespace ScheduleProject.Core.Abstractions.DAL
{
    public interface IRepository<TEntity> : IRepositoryWithSpecifications<TEntity> where TEntity : class
	{
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Delete(TEntity entity, CancellationToken cancellationToken = default);
    }
}
