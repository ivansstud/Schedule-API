using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Abstractions;

namespace ScheduleProject.Infrastructure.DAL.EF;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public override int SaveChanges()
	{
		var modifiedEntries = ChangeTracker
			.Entries<EntityBase>()
			.Where(x => x.State == EntityState.Modified);

		foreach (var entry in modifiedEntries)
		{
			entry.Entity.MarkAsModified();
		}

		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var modifiedEntries = ChangeTracker
			.Entries<EntityBase>()
			.Where(x => x.State == EntityState.Modified);

		foreach (var entry in modifiedEntries)
		{
			entry.Entity.MarkAsModified();
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}
