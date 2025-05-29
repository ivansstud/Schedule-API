using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Enums;
using ScheduleProject.Application.Requests.Schedules;

namespace ScheduleProject.API.Endpoints;

public static class SchedulesEndpoints
{
	public static void MapEndpoints(WebApplication app)
	{
		var group = app.MapGroup("api/v1/schedules")
			.RequireAuthorization();

		group.MapGet("user/{userId:long}", GetByUserHandler);

		group.MapGet("", GetAllHandler)
			.RequireAuthorization(AppPolicies.AdminOnly);

		group.MapPost("", CreateHandler);

		group.MapDelete("", DeleteHandler);
	}

	private static async Task<IResult> GetAllHandler(IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(GetAllSchedulesRequest.Instance, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Ok(result.Value);
	}

	private static async Task<IResult> DeleteHandler([FromBody] DeleteScheduleCommand request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.NoContent();
	}

	private static async Task<IResult> GetByUserHandler([AsParameters] GetSchedulesByUserRequest request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Ok(result.Value);
	}

	private static async Task<IResult> CreateHandler(CreateScheduleCommand request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Created();
	}
}
