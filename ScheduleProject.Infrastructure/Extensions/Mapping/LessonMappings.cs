using ScheduleProject.Application.DTOs.Lesson;
using ScheduleProject.Core.Entities;

namespace ScheduleProject.Infrastructure.Extensions.Mapping;

public static class LessonMappings
{
	public static LessonDto MapToFullDto(this Lesson scheduleWeeksType)
	{
		return new LessonDto
		{
			Id = scheduleWeeksType.Id,
			Name = scheduleWeeksType.Name,
			Audience = scheduleWeeksType.Audience,
			TeacherName = scheduleWeeksType.TeacherName,
			Day = scheduleWeeksType.Day,
			Description = scheduleWeeksType.Description,
			EndTime = scheduleWeeksType.EndTime,
			StartTime = scheduleWeeksType.StartTime,
			ScheduleId = scheduleWeeksType.ScheduleId,
			ScheduleWeeksType = scheduleWeeksType.ScheduleWeeksType,
			LessonType = scheduleWeeksType.LessonType,
			IsDeleted = scheduleWeeksType.IsDeleted
		};
	}

	public static LessonByScheduleDto MapToByScheduleDto(this Lesson lesson)
	{
		return new LessonByScheduleDto
		{
			Id = lesson.Id,
			Name = lesson.Name,
			Day = lesson.Day,
			EndTime = lesson.EndTime,
			Audience = lesson.Audience,
			StartTime = lesson.StartTime,
			Description = lesson.Description,
			TeacherName = lesson.TeacherName,
			LessonType = lesson.LessonType.Name,
			ScheduleWeeksType = lesson.ScheduleWeeksType
		};
	}
}
