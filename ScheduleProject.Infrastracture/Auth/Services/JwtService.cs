using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Abstractions.Services;
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

	public string CreateAccessToken(long userId, string userName, List<string> userRoles)
	{
		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, userName),
			new (ClaimTypes.NameIdentifier, userId.ToString()),
		};

		claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
		var tokenDescriptor = new JwtSecurityToken(
			claims: claims,
			issuer: _jwtOptions.Issuer,
			audience: _jwtOptions.Audience,
			signingCredentials: credentials,
			expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes));

		return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
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
