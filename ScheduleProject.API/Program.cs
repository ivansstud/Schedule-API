using Microsoft.AspNetCore.CookiePolicy;
using Scalar.AspNetCore;
using ScheduleProject.Application.Services;
using ScheduleProject.Core.Abstractions.Services;
using ScheduleProject.Infrastracture;
using ScheduleProject.Infrastracture.Auth.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddMSSQLDbContext(configuration.GetConnectionString("MSSQL")!);

builder.Services.AddRepositories();

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

builder.Services.AddJwtAuthenication(jwtOptions);

builder.Services.AddAuthorization();

builder.Services.AddScoped<IScheduleService, ScheduleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Strict,
	HttpOnly = HttpOnlyPolicy.Always,
	Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
