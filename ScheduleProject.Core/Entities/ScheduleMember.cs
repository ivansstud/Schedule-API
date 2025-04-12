using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Enums;

#pragma warning disable CS8618

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
	public Schedule Schedule { get; private set; }

	public long TelegramUserId { get; private set; }
	public TelegramUser TelegramUser { get; private set; }

	public ScheduleRole Role { get; private set; }

	public static Result<ScheduleMember> Create(long scheduleId, long telegramUserId, ScheduleRole role)
	{
		return new ScheduleMember(scheduleId, telegramUserId, role);
	}
}
