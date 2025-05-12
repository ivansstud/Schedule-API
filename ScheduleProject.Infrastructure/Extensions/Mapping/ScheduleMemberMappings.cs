using ScheduleProject.Application.DTOs.Schedules;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastructure.Extensions.Mapping;

public static class ScheduleMemberMappings
{
	public static ScheduleMemberDto MapToDto(this ScheduleMember member)
	{
		return new ScheduleMemberDto
		{
			UserId = member.UserId,
			Role = member.Role.Name
		};
	}

	public static ScheduleByUserDto MapToScheduleByUserDto(this ScheduleMember member)
	{
		return new ScheduleByUserDto
		{
			Id = member.ScheduleId,
			Name = member.Schedule.Name,
			Type = member.Schedule.Type.Name,
			Role = member.Role.Name
		};
	}
}