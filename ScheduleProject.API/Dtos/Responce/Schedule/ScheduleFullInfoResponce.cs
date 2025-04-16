using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Responce.Schedule;

public sealed class ScheduleFullInfoResponce
{
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public ScheduleType Type { get; set; }
	public ScheduleWeeksType WeeksType { get; set; }
	public List<ScheduleMemberResponce> Members { get; set; } = [];
	public List<LessonResponce> Lessons { get; set; } = [];
}
