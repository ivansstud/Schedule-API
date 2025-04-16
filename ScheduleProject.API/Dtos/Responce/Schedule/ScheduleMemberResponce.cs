using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Responce.Schedule;

public sealed class ScheduleMemberResponce
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public ScheduleRole Role { get; set; }
}

