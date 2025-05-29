namespace ScheduleProject.Application.DTOs.Schedules;

public class ScheduleByUserDto
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string Role { get; set; } = null!;
	public string Type { get; set; } = null!;
}
