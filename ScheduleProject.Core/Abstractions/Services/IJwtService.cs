namespace ScheduleProject.Core.Abstractions.Services;

public interface IJwtService
{
	string CreateAccessToken(long userId, string userName, List<string> userRoles);
	string CreateRefreshToken();
}
