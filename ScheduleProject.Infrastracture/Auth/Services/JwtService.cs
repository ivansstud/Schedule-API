using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Dtos.Auth;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastracture.Auth.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ScheduleProject.Infrastracture.Auth.Services;

public class JwtService : IJwtService
{
	private readonly JwtOptions _jwtOptions;

	public JwtService(IOptions<JwtOptions> options)
	{
		_jwtOptions = options.Value;
	}

	public string CreateAccessToken(CreateAccessTokenDto createTokenDto)
	{
		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, createTokenDto.Name),
			new (CustomClaimTypes.Login, createTokenDto.Login),
			new (ClaimTypes.NameIdentifier, createTokenDto.UserId.ToString())
		};

		claims.AddRange(createTokenDto.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

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
			var principal = handler.ValidateToken(token, parameters, out var validatedToken);
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
