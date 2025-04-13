using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Infrastracture.Auth.Options;
using ScheduleProject.Infrastracture.Auth.Services;
using ScheduleProject.Infrastracture.EF;
using System.Text;

namespace ScheduleProject.Infrastracture;

public static class DependencyInjection
{
	public static void AddMSSQLDbContext(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<AppDbContext>(builder =>
		{
			builder.UseSqlServer(connectionString);
		});
	}

	public static void AddJwtAuthenication(this IServiceCollection services, JwtOptions jwtOptions)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = jwtOptions.Issuer,
					ValidateAudience = true,
					ValidAudience = jwtOptions.Audience,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
					ValidateIssuerSigningKey = true
				};

				options.Events = new JwtBearerEvents()
				{
					OnMessageReceived = context =>
					{
						context.Token = context.Request.Cookies[jwtOptions.AccessTokenCookieKey];
						return Task.CompletedTask;
					}
				};
			});

		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IJwtService, JwtService>();
		services.AddScoped<IPasswordService, PasswordService>();
	}
}
