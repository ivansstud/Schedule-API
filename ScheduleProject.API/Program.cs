using Microsoft.AspNetCore.CookiePolicy;
using Scalar.AspNetCore;
using ScheduleProject.API.Endpoints;
using ScheduleProject.API.Enums;
using ScheduleProject.Application.Requests.Auth;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Infrastructure;
using ScheduleProject.Infrastructure.Auth.Enums;
using ScheduleProject.Infrastructure.Auth.Options;
using ScheduleProject.Infrastructure.Handlers.Auth;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();

builder.Services.AddControllers();

//builder.Services.AddPostgreSqlDbContext(configuration.GetConnectionString("PostgreSQL")!);
builder.Services.AddMssqlDbContext(configuration.GetConnectionString("MSSQL")!);

builder.Services.AddUnitOfWork();

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;

builder.Services.AddJwtAuthentication(jwtOptions);

builder.Services.AddAuthorizationBuilder()
	.AddPolicy(AppPolicies.AdminOnly, policy =>
	{
		policy.RequireClaim(CustomClaimTypes.Role, AppRoles.Administrator);
	})
	.AddPolicy(AppPolicies.InstitusionsUser, policy =>
	{
		policy.RequireClaim(CustomClaimTypes.Role, AppRoles.InstitusionAdder, AppRoles.InstitusionRemover);
	})
	.AddPolicy(AppPolicies.InstitusionAdder, policy =>
	{
		policy.RequireClaim(CustomClaimTypes.Role, AppRoles.InstitusionAdder);
	})
	.AddPolicy(AppPolicies.InstitusionRemover, policy =>
	{
		policy.RequireClaim(CustomClaimTypes.Role, AppRoles.InstitusionRemover);
	});

builder.Services.AddMediatR(c =>
{
	c.RegisterServicesFromAssemblies(typeof(UserRegisterCommand).Assembly, typeof(UserRegisterHandler).Assembly);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(provider =>
	provider.GetRequiredService<IHttpContextAccessor>().HttpContext?.User
	?? throw new InvalidOperationException("User not available"));


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

app.MapApplicationEndpoints();

app.Run();
