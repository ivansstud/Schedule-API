using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Enums;
using ScheduleProject.Application.Requests.Lessons;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.API.Endpoints;

public static class LessonsEndpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        var authGroup = app.MapGroup("api/v1/lessons")
            .RequireAuthorization();

        authGroup.MapGet("schedule/{scheduleId:long}", GetAllByScheduleHandler);
            
        authGroup.MapPost("", CreateHandler);

        authGroup.MapPut("", UpdateHandler);
            
        authGroup.MapDelete("", DeleteHandler);

        authGroup.MapGet("", GetAllHandler)
            .RequireAuthorization(AppPolicies.AdminOnly);
    }

	private static async Task<IResult> GetAllHandler(IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(GetAllLessonsRequest.Instance, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Ok(result.Value);
	}

    private static async Task<IResult> GetAllByScheduleHandler([AsParameters] GetLessonsByScheduleRequest request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Ok(result.Value);
	}

	private static async Task<IResult> CreateHandler(CreateLessonCommand request, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
            
        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Created();
    }

	private static async Task<IResult> UpdateHandler(UpdateLessonCommand request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.NoContent();
	}

	private static async Task<IResult> DeleteHandler([FromBody] DeleteLessonCommand request, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
            
        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.NoContent();
    }
}