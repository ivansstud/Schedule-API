using ScheduleProject.Infrastracture;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMSSQLDbContext(configuration.GetConnectionString("MSSQL")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
