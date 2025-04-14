using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Core.Entities;

public class ScheduleMember : EntityBase
{
	private ScheduleMember() { } // Для EF Core

	private ScheduleMember(long scheduleId, long userId, ScheduleRole role)
	{
		ScheduleId = scheduleId;
		UserId = userId;
		Role = role;
	}

	public long ScheduleId { get; private set; }
	public Schedule Schedule { get; private set; } = null!;

	public long UserId { get; private set; }
	public AppUser User { get; private set; } = null!;

	public ScheduleRole Role { get; private set; }

	public static Result<ScheduleMember> Create(long scheduleId, long userId, ScheduleRole role)
	{
		var result = new ScheduleMember(scheduleId, userId, role);
		return result;
	}
}
