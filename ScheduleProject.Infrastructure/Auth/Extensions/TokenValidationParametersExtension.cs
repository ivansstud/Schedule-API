using Microsoft.IdentityModel.Tokens;
using System.Text;
using ScheduleProject.Infrastructure.Auth.Options;
using ScheduleProject.Infrastructure.Auth.Enums;

namespace ScheduleProject.Infrastructure.Auth.Extensions;

public static class TokenValidationParametersExtension
{
	public static TokenValidationParameters CreateParameters(this TokenValidationParameters validationParameters, JwtOptions options)
	{
		return new TokenValidationParameters()
		{
			ValidateIssuer = true,
			ValidIssuer = options.Issuer,
			ValidateAudience = true,
			ValidAudience = options.Audience,
			ValidateLifetime = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
			ValidateIssuerSigningKey = true,
			RoleClaimType = CustomClaimTypes.Role
		};
	}
}
