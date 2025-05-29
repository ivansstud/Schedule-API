using ScheduleProject.Core.Entities;

namespace ScheduleProject.Core.Abstractions.Services;

public interface IAuthService
{
	public string? GetCurrentAccessToken();

	public string? GetCurrentRefreshToken();

	public void AddTokensToClient(string accessToken, string refreshToken);

	public void RemoveTokensFromClient();

	bool TryGetLoginFromExpiredToken(string? accessToken, out string login);

	void UpdateAuthToken(AppUser user);

	AuthToken CreateAuthToken(long userId, string login, string name, List<string> roles);
}
