using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;
using ScheduleProject.Core.Entities.Enums;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities;

public class Schedule : EntityBase
{
	public const int MaxNameLength = 32;
	public const int MaxDescriptionLength = 128;

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
		if (name.Length > MaxNameLength)
		{
			return Result.Failure<Schedule>($"Название не может быть длиннее {MaxNameLength} символов");
		}
		if (description?.Length > MaxDescriptionLength)
		{
			return Result.Failure<Schedule>($"Описание не может быть длиннее {MaxDescriptionLength} символов");
		}
		if (type == ScheduleType.Custom && institutionId != null)
		{
			return Result.Failure<Schedule>($"Оригинальное расписание не может быть создано с учреждением");
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
}
