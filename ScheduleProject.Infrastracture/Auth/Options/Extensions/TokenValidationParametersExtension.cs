using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ScheduleProject.Infrastracture.Auth.Options.Extensions;

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
			ValidateIssuerSigningKey = true
		};
	}
}
