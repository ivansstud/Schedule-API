using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Abstractions;

namespace ScheduleProject.Infrastracture.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<AppUser> Users { get; set; }
	public DbSet<Lesson> Lessons { get; set; }
	public DbSet<Institution> Institutions { get; set; }
	public DbSet<ScheduleMember> ScheduleMembers { get; set; }
	public DbSet<Schedule> Schedules { get; set; }
	public DbSet<UserRole> UserRoles { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	public override int SaveChanges()
	{
		var modifiedEntries = ChangeTracker.Entries<EntityBase>().Where(x => x.State == EntityState.Modified);

		foreach (var entry in modifiedEntries)
		{
			entry.Entity.MarkAsModified();
		}

		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		var modifiedEntries = ChangeTracker.Entries<EntityBase>().Where(x => x.State == EntityState.Modified);

		foreach (var entry in modifiedEntries)
		{
			entry.Entity.MarkAsModified();
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}
