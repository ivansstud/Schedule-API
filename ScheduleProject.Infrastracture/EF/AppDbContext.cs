using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastracture.EF;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<TelegramUser> TelegramUsers { get; set; }
	public DbSet<Lesson> Lessons { get; set; }
	public DbSet<Institution> Institutions { get; set; }
	public DbSet<ScheduleMember> ScheduleMembers { get; set; }
	public DbSet<Schedule> Schedules { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
