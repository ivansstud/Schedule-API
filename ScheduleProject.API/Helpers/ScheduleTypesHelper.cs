using ScheduleProject.Core.Entities.Enums;

namespace ScheduleProject.API.Helpers;

public class ScheduleTypesHelper
{
	public static string GetName(ScheduleType type)
	{
		return type switch
		{
			ScheduleType.College => "Колледж",
			ScheduleType.School => "Школа",
			ScheduleType.University => "Унивеситет",
			ScheduleType.Custom => "Оригинальное",
			_ => "Оригинальное"
		};
	}
}
