using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;

#pragma warning disable CS8618

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

	public static ScheduleMember Create(long scheduleId, long userId, ScheduleRole role)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(scheduleId);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

		return new ScheduleMember(scheduleId, userId, role);
	}
}
