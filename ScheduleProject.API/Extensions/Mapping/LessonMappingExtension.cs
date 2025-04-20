using ScheduleProject.API.Dtos.Responce.Lesson;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.API.Extensions.Mapping;

public static class LessonMappingExtension
{
	public static LessonInfoResponce MapToInfoResponce(this Lesson lesson)
	{
		return new LessonInfoResponce
		{
			Id  = lesson.Id,
			Name = lesson.Name,
			Audience = lesson.Audience,
			TeacherName = lesson.TeacherName,
			Description = lesson.Description,
			LessonType = lesson.LessonType.Value,
			StartTime = lesson.StartTime,
			EndTime = lesson.EndTime,
			SheduleWeeksType = lesson.SheduleWeeksType,
			Day = lesson.DayOfWeek,
		};
	}
}
