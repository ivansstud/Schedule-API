using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Enums;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.Auth.Options;
using ScheduleProject.Infrastructure.DAL.Services;
using System.Net.Http;

namespace ScheduleProject.Infrastructure.Auth.Services;

public class AuthService : IAuthService
{
	private readonly JwtOptions _jwtOptions;
	private readonly IJwtService _jwtService;
	private readonly IHttpContextAccessor _accessor;

	public AuthService(IJwtService jwtService, IOptions<JwtOptions> jwtOptions, IHttpContextAccessor accessor)
	{
		_jwtService = jwtService;
		_jwtOptions = jwtOptions.Value;
		_accessor = accessor;
	}

	public string? GetCurrentAccessToken()
	{
		return _accessor.HttpContext?.Request.Cookies[_jwtOptions.AccessTokenCookieKey];
	}

	public string? GetCurrentRefreshToken()
	{
		return _accessor.HttpContext?.Request.Cookies[_jwtOptions.RefreshTokenCookieKey];
	}

	public void AddTokensToClient(string accessToken, string refreshToken)
	{
		_accessor.HttpContext?.Response.Cookies.Append(_jwtOptions.AccessTokenCookieKey, accessToken);
		_accessor.HttpContext?.Response.Cookies.Append(_jwtOptions.RefreshTokenCookieKey, refreshToken);
	}

	public void RemoveTokensFromClient()
	{
		_accessor.HttpContext?.Response.Cookies.Delete(_jwtOptions.AccessTokenCookieKey);
		_accessor.HttpContext?.Response.Cookies.Delete(_jwtOptions.RefreshTokenCookieKey);
	}

	public bool TryGetLoginFromExpiredToken(string? accessToken, out string login)
	{
		login = string.Empty;

		if (string.IsNullOrEmpty(accessToken))
		{
			return false;
		}
		
		var validationParameters = new TokenValidationParameters().CreateParameters(_jwtOptions);
		validationParameters.ValidateLifetime = false;

		var principalResult = _jwtService.GetPrincipalFromToken(accessToken, validationParameters);

		if (principalResult.IsFailure)
		{
			return false;
		}
		
		var foundLogin = principalResult.Value.FindFirst(CustomClaimTypes.Login)?.Value;

		if (foundLogin == null)
		{
			return false;
		}
		
		login = foundLogin;
		return true;
	}

	public void UpdateAuthToken(AppUser user)
	{
		var roles = user.Roles.Select(x => x.Name).ToList();
		var newAccessToken = _jwtService.CreateAccessToken(user.Id, user.Login, user.FirstName, roles);
		var newRefreshToken = _jwtService.CreateRefreshToken();

		user.AuthToken!.Update(
			newAccessToken,
			newRefreshToken,
			_jwtOptions.AccessTokenExpiryMinutes,
			_jwtOptions.RefreshTokenExpiryDays
		);
	}

	public AuthToken CreateAuthToken(long userId, string login, string name, List<string> roles)
	{
		var accessToken = _jwtService.CreateAccessToken(userId, login, name, roles);
		var refreshToken = _jwtService.CreateRefreshToken();
		var authToken = AuthToken.Create(
			ownerId: userId,
			accessToken: accessToken,
			refreshToken: refreshToken,
			accessTokenExpiryDate: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
			refreshTokenExpiryDate: DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
		);

		return authToken;
	}
}
