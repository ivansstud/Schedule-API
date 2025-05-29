using MediatR;
using Microsoft.AspNetCore.Mvc;
using ScheduleProject.API.Enums;
using ScheduleProject.Application.Requests.Institusions;

namespace ScheduleProject.API.Endpoints;

public static class InstitusionsEndpoints
{
	public static void MapEndpoints(WebApplication app)
	{
		var group = app.MapGroup("api/v1/institusions")
			.RequireAuthorization(AppPolicies.InstitusionsUsers);

		group.MapGet("", GetInstitusionsHandler);

		group.MapPost("", CreateInstitusionHandler)
			.RequireAuthorization();

		group.MapPut("", UpdateInstitusionHandler);

		group.MapDelete("", DeleteInstitusionHandler);
	}

	private static async Task<IResult> GetInstitusionsHandler([AsParameters] GetInstituisonsRequest request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Ok(result.Value);
	}

	private static async Task<IResult> CreateInstitusionHandler(CreateInstitusionCammand request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.Created();
	}

	private static async Task<IResult> UpdateInstitusionHandler(UpdateInstitusionCommand request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.NoContent();
	}

	private static async Task<IResult> DeleteInstitusionHandler([FromBody] DeleteInstitusionCommand request, IMediator mediator, CancellationToken cancellationToken)
	{
		var result = await mediator.Send(request, cancellationToken);

		if (result.IsFailure)
		{
			return Results.BadRequest(result.Error);
		}

		return Results.NoContent();
	}
}
