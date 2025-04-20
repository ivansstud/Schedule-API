using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Core.Dtos.Lesson;

public sealed class CreateLessonDto
{
	public long ScheduleId { get; set; }
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
