using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Core.Abstractions.DAL;
using ScheduleProject.Infrastracture.DAL.EF;

namespace ScheduleProject.Infrastracture.DAL.Repositories;

public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
	private readonly AppDbContext _dbContext;

	public RepositoryBase(AppDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		await _dbContext.AddAsync(entity, cancellationToken);
		return entity;
	}

	public void Remove(TEntity entity)
	{
		_dbContext.Remove(entity);
	}

	public async Task<List<TEntity>> GetListBySpecificationAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<TEntity>().WithSpecification(specification).ToListAsync(cancellationToken);
	}

	public async Task<TEntity?> GetSingleBySpecificationAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<TEntity>().WithSpecification(specification).SingleOrDefaultAsync(cancellationToken);
	}

	public async Task CommitAsync(CancellationToken cancellationToken = default)
	{
		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
