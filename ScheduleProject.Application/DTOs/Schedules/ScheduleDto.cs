using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Application.DTOs.ValueObjects;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Application.DTOs.Schedules;

public class ScheduleDto
{
	public long Id { get; set; }
	public long OwnerId { get; set; }
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public ScheduleTypeDto Type { get; set; } = null!;
	public ScheduleWeeksTypeDto WeeksType { get; set; } = null!;
	public long? InstitutionId { get; set; }
	public ScheduleMemberDto[] Members { get; set; } = [];
	public LessonDto[] Lessons { get; set; } = [];
}
