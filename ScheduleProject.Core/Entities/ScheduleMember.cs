using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.Core.Entities;

public class ScheduleMember : Entity
{
	private ScheduleMember() { } // Для EF Core

	private ScheduleMember(long scheduleId, long telegramUserId, ScheduleRole role)
	{
		ScheduleId = scheduleId;
		TelegramUserId = telegramUserId;
		Role = role;
	}

	public long ScheduleId { get; private set; }
	public Schedule Schedule { get; private set; } = null!;

	public long TelegramUserId { get; private set; }
	public AppUser TelegramUser { get; private set; } = null!;

	public ScheduleRole Role { get; private set; }

	public static Result<ScheduleMember> Create(long scheduleId, long telegramUserId, ScheduleRole role)
	{
		return new ScheduleMember(scheduleId, telegramUserId, role);
	}
}
