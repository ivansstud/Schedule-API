using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Abstractions.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ScheduleProject.Infrastructure.Auth.Options;
using ScheduleProject.Infrastructure.Auth.Enums;

namespace ScheduleProject.Infrastructure.Auth.Services;

public class JwtService : IJwtService
{
	private readonly JwtOptions _jwtOptions;

	public JwtService(IOptions<JwtOptions> options)
	{
		_jwtOptions = options.Value;
	}

	public string CreateAccessToken(long userId, string login, string firstName, List<string> roles)
	{
		var claims = new List<Claim>
		{
			new (CustomClaimTypes.FirstName, firstName),
			new (CustomClaimTypes.Login, login),
			new (CustomClaimTypes.Id, userId.ToString())
		};

		claims.AddRange(roles.Select(role => new Claim(CustomClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
		var tokenDescriptor = new JwtSecurityToken(
			claims: claims,
			issuer: _jwtOptions.Issuer,
			audience: _jwtOptions.Audience,
			signingCredentials: credentials,
			expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes)
		);

		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
	}

	public Result<ClaimsPrincipal> GetPrincipalFromToken(string token, TokenValidationParameters parameters)
	{
		var handler = new JwtSecurityTokenHandler();
		try
		{
			var principal = handler.ValidateToken(token, parameters, out _);
			if (principal is null)
			{
				return Result.Failure<ClaimsPrincipal>("Не удалось индентифицировать пользователя");
			}

			return principal;
		}
		catch (Exception)
		{
			return Result.Failure<ClaimsPrincipal>("Не удалось индентифицировать пользователя");
			//TODO: добавить логирование ошибок
		}
	} 

	public string CreateRefreshToken()
	{
		var bytes = new byte[32];
		using var random = RandomNumberGenerator.Create();
		random.GetBytes(bytes);
		var refreshToken = Convert.ToBase64String(bytes);

		return refreshToken;
	}
}
