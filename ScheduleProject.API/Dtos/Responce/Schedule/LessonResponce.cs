using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Responce.Schedule;

public sealed class LessonResponce
{
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public string? Audience { get; set; }
	public string Type { get; set; } = null!;
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public ScheduleWeeksType WeeksType { get; set; }
	public DayOfWeek Day { get; set; }
}