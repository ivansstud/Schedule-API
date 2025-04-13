namespace ScheduleProject.Infrastracture.Auth.Options;

public class JwtOptions
{
	public required string SecretKey { get; set; }
	public required string Audience { get; set; }
	public required string Issuer { get; set; }
	public required string AccessTokenCookieKey { get; set; }
	public required string RefreshTokenCookieKey { get; set; }
	public required int AccessTokenExpiryMinutes { get; set; }
	public required int RefreshTokenExpiryDays { get; set; }
}

