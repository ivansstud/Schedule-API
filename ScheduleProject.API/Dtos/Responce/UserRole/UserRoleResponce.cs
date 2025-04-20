namespace ScheduleProject.API.Dtos.Responce.UserRole;

public sealed class UserRoleResponce
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public bool IsDeleted { get; set; }
}
