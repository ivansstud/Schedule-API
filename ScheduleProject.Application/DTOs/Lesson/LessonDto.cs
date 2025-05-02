using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Application.DTOs.Lesson;

public class LessonDto
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public string? TeacherName { get; set; }
	public string? Audience { get; set; }
	public LessonType LessonType { get; set; } = null!;
	public TimeOnly StartTime { get; set; }
	public TimeOnly EndTime { get; set; }
	public ScheduleWeeksType ScheduleWeeksType { get; set; } = null!;
	public Day Day { get; set; } = null!;
	public long ScheduleId { get; set; }
	public bool IsDeleted { get; set; }
}
