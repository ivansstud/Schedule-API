using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;
using ScheduleProject.Core.Entities.ValueObjects;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities;

public class Schedule : EntityBase
{
	public const int MaxNameLength = 32;
	public const int MaxDescriptionLength = 128;

	public const int MinNameLength = 1;

	private readonly List<ScheduleMember> _members = [];
	private readonly List<Lesson> _lessons = [];

	private Schedule() { } // Для EF Core

	private Schedule(string name, string? description, ScheduleType type, ScheduleWeeksType weeksType, long? institutionId)
	{
		Name = name;
		Description = description;
		Type = type;
		WeeksType = weeksType;
		InstitutionId = institutionId;
	}

	public string Name { get; private set; }
	public string? Description { get; private set; }
	public ScheduleType Type { get; private set; }
	public ScheduleWeeksType WeeksType { get; private set; }
	public long? InstitutionId { get; private set; }
	public Institution? Institution { get; private set; }
	public IReadOnlyList<ScheduleMember> Members => _members;
	public IReadOnlyList<Lesson> Lessons => _lessons;

	public static Result<Schedule> Create(string name, string? description, ScheduleType type, ScheduleWeeksType weeksType, long? institutionId)
	{
		name = name.Trim();

		Result[] validationResults = [
			ValidateName(name),
			ValidateDescription(description),
			ValidateScheduleType(type, institutionId)
		];

		if (validationResults.Any(x => x.IsFailure))
		{
			return Result.Failure<Schedule>(validationResults.First(x => x.IsFailure).Error);
		}

		var result = new Schedule(name, description, type, weeksType, institutionId);
		return result;
	}

	public void AddLesson(Lesson lesson)
	{
		if (!_lessons.Any(x => x.Id == lesson.Id))
		{
			_lessons.Add(lesson);
		}
	}

	public void AddMember(ScheduleMember member)
	{
		if (!_members.Any(x => x.Id == member.Id))
		{
			_members.Add(member);
		}
	}

	public long GetCreatorId()
	{
		return _members.First(x => x.Role == ScheduleRole.Creator).Id;
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

	public Result SetDescription(string description)
	{
		if (ValidateName(description).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		Description = description;
		return Result.Success();
	}

	private static Result ValidateName(string name)
	{
		if (name.Length > MaxNameLength || name.Length < MinNameLength)
		{
			return Result.Failure($"Название учреждения должно содержать от {MinNameLength} до {MaxNameLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateDescription(string? description)
	{
		if (description?.Length > MaxDescriptionLength)
		{
			return Result.Failure<Schedule>($"Описание не может быть длиннее {MaxDescriptionLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateScheduleType(ScheduleType? type, long? institutionId)
	{
		if (type == ScheduleType.Custom && institutionId != null)
		{
			return Result.Failure<Schedule>($"Оригинальное расписание не может быть создано с учреждением");
		}

		return Result.Success();
	}
}
