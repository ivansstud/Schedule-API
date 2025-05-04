using System.Security.Claims;
using MediatR;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Infrastructure.Auth.Extensions;

namespace ScheduleProject.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapEndpoints(WebApplication app)
    {
        var authGroup = app.MapGroup("api/v1/auth");

        authGroup.MapPost("register", RegisterHandler);

        authGroup.MapPost("login", LoginHandler);

        authGroup.MapPost("logout", LogoutHandler);

        authGroup.MapPost("refresh", RefreshTokenHandler);
            
        authGroup.MapPost("current", GetCurrentIdentityHandler)
            .RequireAuthorization();
    }
        
    private static async Task<IResult> LoginHandler(UserLoginCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
		var loginCommand = await mediator.Send(command, cancellationToken);

		if (loginCommand.IsFailure)
		{
			return Results.BadRequest(loginCommand.Error);
		}

        return Results.NoContent();
    }
        
    private static async Task<IResult> RegisterHandler(UserRegisterCommand command, IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Created();
    }
        
    private static async Task<IResult> LogoutHandler(IMediator mediator, CancellationToken cancellationToken)
    {
        await mediator.Send(UserLogoutRequest.Instance, cancellationToken);

        return Results.NoContent();
    }
        
    private static async Task<IResult> RefreshTokenHandler(IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(RefreshTokensCommand.Instance, cancellationToken);
            
        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok();
    }
        
    private static IResult GetCurrentIdentityHandler(ClaimsPrincipal user)
    {
        return Results.Ok(new
        {
            Id = user.GetId(),
            FirstName = user.GetFirstName(),
            Login = user.GetLogin(),
            Roles = user.GetRoles(),
        });
    }
}