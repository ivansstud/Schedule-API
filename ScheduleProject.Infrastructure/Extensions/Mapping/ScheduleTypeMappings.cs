using ScheduleProject.Application.DTOs.ValueObjects;
using ScheduleProject.Core.Entities.ValueObjects;

namespace ScheduleProject.Infrastructure.Extensions.Mapping;

public static class ScheduleTypeMappings
{
	public static ScheduleTypeDto MapToDto(this ScheduleType scheduleType)
	{
		return new ScheduleTypeDto
		{
			Name = scheduleType.Name,
			Value = scheduleType.Value
		};
	}
}
