using CSharpFunctionalExtensions;

namespace ScheduleProject.Core.Entities;

public class AuthToken : Entity
{
	private AuthToken(long ownerId, string accessToken, string refreshToken, DateTime accessTokenExpiryDate, DateTime refreshTokenExpiryDate)
	{
		OwnerId = ownerId;
		AccessToken = accessToken;
		RefreshToken = refreshToken;
		AccessTokenExpiryDate = accessTokenExpiryDate;
		RefreshTokenExpiryDate = refreshTokenExpiryDate;
	}

	public long OwnerId { get; private set; }
	public AppUser Owner { get; private set; } = null!;
	public string AccessToken { get; private set; }
	public string RefreshToken { get; private set; }
	public DateTime AccessTokenExpiryDate { get; private set; }
	public DateTime RefreshTokenExpiryDate { get; private set; }


	/// <exception cref="InvalidOperationException"></exception>
	public static AuthToken Create(long ownerId,
		string accessToken,
		string refreshToken,
		DateTime accessTokenExpiryDate,
		DateTime refreshTokenExpiryDate)
	{
		if (string.IsNullOrEmpty(accessToken.Trim()))
		{
			throw new InvalidOperationException("AccessToken не может быть пустым");
		}
		if (string.IsNullOrEmpty(refreshToken.Trim()))
		{
			throw new InvalidOperationException("RefreshToken не может быть пустым");
		}
		if (accessTokenExpiryDate < DateTime.UtcNow)
		{
			throw new InvalidOperationException("AccessTokenExpiryDate не может быть меньше текущего времени");
		}
		if (refreshTokenExpiryDate < DateTime.UtcNow)
		{
			throw new InvalidOperationException("RefreshTokenExpiryDate не может быть меньше текущего времени");
		}

		return new (ownerId, accessToken, refreshToken, accessTokenExpiryDate, refreshTokenExpiryDate);
	}

	public void Update(string accessToken, string refreshToken, int accessTokenExpiryMinutes, int refreshTokenExpiryDays)
	{
		AccessToken = accessToken;
		RefreshToken = refreshToken;
		RefreshTokenExpiryDate = DateTime.Now.AddDays(refreshTokenExpiryDays);
		AccessTokenExpiryDate = DateTime.Now.AddMinutes(accessTokenExpiryMinutes);
	}
}
