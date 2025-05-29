using MediatR;

namespace ScheduleProject.Application.Requests.Auth;

public class UserLogoutRequest : IRequest
{
	public static readonly UserLogoutRequest Instance = new ();

	private UserLogoutRequest()
	{
		
	}
}
