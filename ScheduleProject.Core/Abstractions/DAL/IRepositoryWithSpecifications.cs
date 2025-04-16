using Ardalis.Specification;

namespace ScheduleProject.Core.Abstractions.DAL
{
	public interface IRepositoryWithSpecifications<TEntity> where TEntity : class
	{
		Task<List<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

		Task<TEntity?> GetSingleBySpecificationAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default);
	}
}