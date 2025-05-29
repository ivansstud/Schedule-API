using ScheduleProject.Application.DTOs.ValueObjects;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Infrastructure.Extensions.Mapping;

public static class ScheduleWeeksTypeMappings
{
	public static ScheduleWeeksTypeDto MapToDto(this ScheduleWeeksType scheduleWeeksType)
	{
		return new ScheduleWeeksTypeDto
		{
			Name = scheduleWeeksType.Name,
			Value = scheduleWeeksType.Value
		};
	}
}
