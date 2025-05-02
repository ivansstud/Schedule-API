using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Core.Entities;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastructure.Auth.Enums;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.Auth.Options;

namespace ScheduleProject.API.Endpoints;

public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("api/auth");

            authGroup.MapPost("register", RegisterHandler);

            authGroup.MapPost("login", LoginHandler);

            authGroup.MapPost("logout", LogoutHandler);

            authGroup.MapPost("refresh", RefreshTokenHandler);
            
            authGroup.MapPost("current", GetCurrentIdentityHandler)
                .RequireAuthorization();
        }
        
        private static async Task<IResult> LoginHandler(
            UserLoginCommand command,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
		    var loginCommand = await mediator.Send(command, cancellationToken);

		    if (loginCommand.IsFailure)
		    {
			    return Results.BadRequest(loginCommand.Error);
		    }

            return Results.NoContent();
        }
        
        private static async Task<IResult> RegisterHandler(
            UserRegisterCommand command,
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
        
        private static async Task<IResult> LogoutHandler(
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var request = new UserLogoutRequest();
            await mediator.Send(request, cancellationToken);

            return Results.NoContent();
        }
        
        private static async Task<IResult> RefreshTokenHandler(
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var refreshCommand = new RefreshTokensCommand();
            var result = await mediator.Send(refreshCommand, cancellationToken);
            
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