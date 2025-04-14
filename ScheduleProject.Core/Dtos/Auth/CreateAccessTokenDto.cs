namespace ScheduleProject.Core.Dtos.Auth;

public sealed record CreateAccessTokenDto(string Login, string Name, List<string> Roles);
