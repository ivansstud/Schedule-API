using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Responce.Lesson;

public sealed class LessonInfoResponce
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public string? TeacherName { get; set; }
	public string? Audience { get; set; }
	public string LessonType { get; set; } = null!;
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public ScheduleWeeksType SheduleWeeksType { get; set; }
	public DayOfWeek Day { get; set; }
}
