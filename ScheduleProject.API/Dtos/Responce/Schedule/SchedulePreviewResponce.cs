using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Dtos.Responce.Schedule;

public sealed class SchedulePreviewResponce
{
	public long Id { get; set; }
	public string Name { get; set; } = null!;
	public string RoleName { get; set; } = null!;
	public string TypeName { get; set; } = null!;
}
