using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;
using ScheduleProject.Core.Entities.Enums;
using ScheduleProject.Core.Entities.ValueObjects;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;



#pragma warning disable CS8618 

namespace ScheduleProject.Core.Entities;

public class Lesson : EntityBase
{
	public const int MaxNameLength = 64;
	public const int MaxDescriptionLength = 128;
	public const int MaxTeacherNameLength = 32;
	public const int MaxAudienceLength = 16;

	public const int MinNameLength = 1;

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
		Day day,
		long scheduleId)
	{
		Name = name;
		Description = description;
		TeacherName = teacherName;
		Audience = audience;
		LessonType = lessonType;
		StartTime = startTime;
		EndTime = endTime;
		ScheduleWeeksType = sheduleWeeksType;
		Day = day;
		ScheduleId = scheduleId;
	}

	public string Name { get; private set; }
	public string? Description { get; private set; }
	public string? TeacherName { get; private set; } 
	public string? Audience { get; private set; }
	public LessonType LessonType { get; private set; }
	public TimeOnly StartTime { get; private set; }
	public TimeOnly EndTime { get; private set; }
	public ScheduleWeeksType ScheduleWeeksType { get; private set; }
	public Day Day { get; private set; }

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
		ScheduleWeeksType scheduleWeeksType,
		Day day,
		long scheduleId)
	{
		name = name.Trim();

		Result[] validationResults = [
			ValidateName(name),
			ValidateAudience(audience),
			ValidateTeacherName(teacherName),
			ValidateDescription(description),
			ValidateTime(startTime, endTime),
		];

		if (validationResults.Any(x => x.IsFailure))
		{
			return Result.Failure<Lesson>(validationResults.First(x => x.IsFailure).Error);
		}

		return new Lesson(
			name,
			description,
			teacherName,
			audience,
			lessonType,
			startTime,
			endTime,
			scheduleWeeksType,
			day,
			scheduleId
		); 
	}

	public Result SetName(string name)
	{
		if (ValidateName(name).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		Name = name;
		return Result.Success();
	}

	public Result SetDescription(string? description)
	{
		if (ValidateDescription(description).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		Description = description;
		return Result.Success();
	}

	public Result SetAudience(string? audience)
	{
		if (ValidateAudience(audience).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		Audience = audience;
		return Result.Success();
	}

	public Result SetTeacherName(string? teacherName)
	{
		if (ValidateTeacherName(teacherName).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		TeacherName = teacherName;
		return Result.Success();
	}

	public Result SetTime(TimeOnly startTime, TimeOnly endTime)
	{
		if (ValidateTime(startTime, endTime).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		EndTime = endTime;
		StartTime = startTime;
		return Result.Success();
	}

	public void SetType(LessonType type)
	{
		LessonType = type;
	}

	private static Result ValidateTime(TimeOnly startTime, TimeOnly endTime)
	{
		if (startTime > endTime)
		{
			return Result.Failure<Lesson>($"Время начала занятия не может быть позже времени окончания");
		}

		return Result.Success();
	}

	private static Result ValidateAudience(string? audience)
	{
		if (audience?.Length > MaxAudienceLength)
		{
			return Result.Failure<Lesson>($"Название аудитории не может быть длиннее {MaxAudienceLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateName(string name)
	{
		if (name.Length > MaxNameLength || name.Length < MinNameLength)
		{
			return Result.Failure<Lesson>($"Название занятия должно содержать от {MinNameLength} до {MaxNameLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateDescription(string? description)
	{
		if (description?.Length > MaxDescriptionLength)
		{
			return Result.Failure<Lesson>($"Описание занятия не может быть длиннее {MaxDescriptionLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateTeacherName(string? teacherName)
	{
		if (teacherName?.Length > MaxTeacherNameLength)
		{
			return Result.Failure<Lesson>($"Имя преподавателя не может быть длиннее {MaxTeacherNameLength} символов");
		}

		return Result.Success();
	}
}
