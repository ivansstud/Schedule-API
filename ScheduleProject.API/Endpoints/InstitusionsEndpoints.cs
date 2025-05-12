using MediatR;
using ScheduleProject.API.Enums;
using ScheduleProject.Application.Requests.Institusions;

namespace ScheduleProject.API.Endpoints;

public static class InstitusionsEndpoints
{
	public static void MapEndpoints(WebApplication app)
	{
		var group = app.MapGroup("api/v1/institusions")
			.RequireAuthorization();

		group.MapPost("", CreateInstitusionHandler)
			.RequireAuthorization(AppPolicies.InstitusionAdder);

		group.MapGet("", GetInstitusionsHandler);
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
}
