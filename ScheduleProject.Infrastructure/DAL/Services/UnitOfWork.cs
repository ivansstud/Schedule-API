using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ScheduleProject.Infrastructure.DAL.EF;

namespace ScheduleProject.Infrastructure.DAL.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currenTransaction = null;
    private bool _disposed = false;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public DbContext Db => _context;

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currenTransaction != null)
        {
            return;
        }
        
        _currenTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_currenTransaction == null)
        {
            throw new Exception("Не начатая транзакция не может быть зафиксирована");
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _currenTransaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _currenTransaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await _currenTransaction.DisposeAsync();
            _currenTransaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_currenTransaction == null)
        {
            return;
        }

        try
        {
            await _currenTransaction.RollbackAsync();
        }
        finally
        {
            await _currenTransaction.DisposeAsync();
            _currenTransaction = null;
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        if (disposing)
        {
            if (_currenTransaction != null)
            {
                _currenTransaction.Dispose();
                _currenTransaction = null;
            }
            _context.Dispose();
        }
        _disposed = true;
    }
}