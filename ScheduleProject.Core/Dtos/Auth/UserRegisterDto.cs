namespace ScheduleProject.Core.Dtos.Auth;

public sealed record UserRegisterDto(string Login, string Password, string FirstName, string LastName);