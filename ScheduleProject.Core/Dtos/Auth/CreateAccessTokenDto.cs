namespace ScheduleProject.Core.Dtos.Auth;

public sealed record CreateAccessTokenDto(long UserId, string Login, string Name, List<string> Roles);
