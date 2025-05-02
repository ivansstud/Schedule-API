using MediatR;
using ScheduleProject.Application.Requests.Schedules;

namespace ScheduleProject.API.Endpoints;

public static class SchedulesEndpoints
{
	public static void MapSchedulesEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("api/v1/schedules")
			.RequireAuthorization();

		group.MapPost("", CreateHandler);
	}

	private static async Task<IResult> CreateHandler(
		CreateScheduleCommand command,
		IMediator mediator,
		CancellationToken cancellationToken)
	{
		var result = await mediator.Send(command, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Created();
	}
}
