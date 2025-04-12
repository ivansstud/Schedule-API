using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScheduleProject.Infrastracture.EF;

namespace ScheduleProject.Infrastracture;

public static class DependencyInjection
{
	public static void AddMSSQLDbContext(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<AppDbContext>(builder =>
		{
			builder.UseSqlServer(connectionString);
		});
	}
}
