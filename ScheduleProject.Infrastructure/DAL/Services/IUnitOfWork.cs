using Microsoft.EntityFrameworkCore;

namespace ScheduleProject.Infrastructure.DAL.Services;

public interface IUnitOfWork : IDisposable
{
    DbContext Db { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}