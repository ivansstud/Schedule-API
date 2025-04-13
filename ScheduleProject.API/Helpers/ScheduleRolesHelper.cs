using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Helpers;

public static class ScheduleRolesHelper
{
	public static string GetName(ScheduleRole role)
	{
		return role switch
		{
			ScheduleRole.Moderator => "Модератор",
			ScheduleRole.Creator => "Создатель",
			ScheduleRole.Member => "Участник",
			_ => "Участник"
		};
	}
}
