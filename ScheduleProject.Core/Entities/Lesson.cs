using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;

#pragma warning disable CS8618 

namespace ScheduleProject.Core.Entities;

public class Lesson : Entity
{
	public const int MaxNameLength = 64;
	public const int MaxDescriptionLength = 128;
	public const int MaxTeacherNameLength = 32;
	public const int MaxAudienceLength = 16;

	private Lesson() { } // Для EF Core

	private Lesson(
		string name,
		string? description,
		string? teacherName,
		string? audience,
		LessonType lessonType,
		TimeOnly startTime,
		TimeOnly endTime,
		ScheduleWeeksType sheduleWeeksType,
		DayOfWeek dayOfWeek,
		long scheduleId
		)
	{
		Name = name;
		Description = description;
		TeacherName = teacherName;
		Audience = audience;
		LessonType = lessonType;
		StartTime = startTime;
		EndTime = endTime;
		SheduleWeeksType = sheduleWeeksType;
		DayOfWeek = dayOfWeek;
		ScheduleId = scheduleId;
	}

	public string Name { get; private set; }
	public string? Description { get; private set; }
	public string? TeacherName { get; private set; } 
	public string? Audience { get; private set; }
	public LessonType LessonType { get; private set; }
	public TimeOnly StartTime { get; private set; }
	public TimeOnly EndTime { get; private set; }
	public ScheduleWeeksType SheduleWeeksType { get; private set; }
	public DayOfWeek DayOfWeek { get; private set; }

	public long ScheduleId { get; private set; }
	public Schedule Schedule { get; private set; }

	public static Result<Lesson> Create(
		string name,
		string? description,
		string? teacherName,
		string? audience,
		LessonType lessonType,
		TimeOnly startTime,
		TimeOnly endTime,
		ScheduleWeeksType sheduleWeeksType,
		DayOfWeek dayOfWeek,
		long scheduleId
		)
	{
		if (name.Length > MaxNameLength)
		{
			return Result.Failure<Lesson>($"Название занятия не может быть длиннее {MaxNameLength} символов");
		}
		if (description?.Length > MaxDescriptionLength)
		{
			return Result.Failure<Lesson>($"Описание занятия не может быть длиннее {MaxDescriptionLength} символов");
		}
		if (teacherName?.Length > MaxTeacherNameLength)
		{
			return Result.Failure<Lesson>($"Имя преподавателя не может быть длиннее {MaxTeacherNameLength} символов");
		}
		if (audience?.Length > MaxAudienceLength)
		{
			return Result.Failure<Lesson>($"Название аудитории не может быть длиннее {MaxAudienceLength} символов");
		}
		if (startTime > endTime)
		{
			return Result.Failure<Lesson>($"Время начала занятия не может быть позже времени окончания");
		}

		return new Lesson(name,
			description,
			teacherName,
			audience,
			lessonType,
			startTime,
			endTime,
			sheduleWeeksType,
			dayOfWeek,
			scheduleId
		);
	}
}
