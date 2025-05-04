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
}