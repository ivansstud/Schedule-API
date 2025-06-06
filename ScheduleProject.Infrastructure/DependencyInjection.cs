﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Core.Entities;
using ScheduleProject.Infrastructure.Auth.Extensions;
using ScheduleProject.Infrastructure.Auth.Options;
using ScheduleProject.Infrastructure.Auth.Services;
using ScheduleProject.Infrastructure.DAL.EF;
using ScheduleProject.Infrastructure.DAL.Services;

namespace ScheduleProject.Infrastructure;

public static class DependencyInjection
{
	public static void AddMssqlDbContext(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<AppDbContext>(builder =>
		{
			builder.UseSqlServer(connectionString);
		});
	}

	public static void AddPostgreSqlDbContext(this IServiceCollection services, string connectionString)
	{
		services.AddDbContext<AppDbContext>(builder =>
		{
			builder.UseNpgsql(connectionString);
		});
	}

	public static void AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters().CreateParameters(jwtOptions);

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

	public static void AddUnitOfWork(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();

	}

	public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
	{
		using var scope = applicationBuilder.ApplicationServices.CreateScope();
		using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		dbContext.Database.Migrate();
	}
}