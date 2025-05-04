namespace ScheduleProject.API.Endpoints;

public static class InstitusionsEndpoints
{
	public static void MapEndpoints(WebApplication app)
	{
		app.MapGroup("institusions")
			.RequireAuthorization()
	}
}
