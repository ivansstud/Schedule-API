using MediatR;
using Microsoft.Extensions.Logging;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Core.Abstractions.Services;

namespace ScheduleProject.Infrastructure.Handlers.Auth;

public class UserLogoutHandler : IRequestHandler<UserLogoutRequest>
{
	private readonly IAuthService _authService;
	private readonly ILogger<UserLoginHandler> _logger;

	public UserLogoutHandler(IAuthService authService, ILogger<UserLoginHandler> logger)
	{
		_authService = authService;
		_logger = logger;
	}

	public async Task Handle(UserLogoutRequest request, CancellationToken cancellationToken)
	{
		try
		{
			_authService.RemoveTokensFromClient();
			await Task.CompletedTask;
		}
		catch (Exception ex)
		{
			_logger.LogError("{Exception}:", ex.Message + ex.InnerException?.Message);
		}
	}
}
