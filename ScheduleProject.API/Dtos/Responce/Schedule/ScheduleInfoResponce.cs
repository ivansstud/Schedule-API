using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Responce.Schedule;

public sealed class ScheduleInfoResponce
{
	public string Name { get; set; } = null!;
	public string? Description { get; set; }
	public ScheduleType Type { get; set; }
	public ScheduleWeeksType WeeksType { get; set; }
	public long? InstitutionId { get; set; }
}
