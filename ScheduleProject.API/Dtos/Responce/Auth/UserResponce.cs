namespace ScheduleProject.API.Dtos.Responce.Auth;

public sealed class UserResponce
{
	public string Login { get; set; } = "";
	public string FirstName { get; set; } = "";
	public string LastName { get; set; } = "";
	public string[] Roles { get; set; } = [];
}
