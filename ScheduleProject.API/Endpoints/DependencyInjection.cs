namespace ScheduleProject.API.Endpoints;

public static class DependencyInjection
{
	public static void MapApplicationEndpoints(this WebApplication app)
	{
		AuthEndpoints.MapEndpoints(app);
		LessonsEndpoints.MapEndpoints(app);
		SchedulesEndpoints.MapEndpoints(app);
	}
}
