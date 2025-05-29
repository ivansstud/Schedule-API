using CSharpFunctionalExtensions;
using MediatR;

namespace ScheduleProject.Application.Requests.Auth;

public class RefreshTokensCommand : IRequest<Result>
{
	public readonly static RefreshTokensCommand Instance = new ();

	private RefreshTokensCommand()
	{
		
	}
}