using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;

#pragma warning disable CS8618 

namespace ScheduleProject.Core.Entities;

public class Institution : EntityBase
{
	public const int MaxNameLength = 128;
	public const int MaxShortNameLength = 32;
	public const int MaxDescriptionLength = 256;
	
	public const int MinNameLength = 32;
	public const int MinShortNameLength = 3;

	private readonly List<Schedule> _schedules = [];

	private Institution() { } // Для EF Core

	private Institution(string name, string shortName, string? description)
	{
		Name = name;
		ShortName = shortName;
		Description = description;
	}

	public string Name { get; private set; }
	public string ShortName { get; private set; }
	public string? Description { get; private set; }
	public long OwnerId { get; private set; }
	public AppUser Owner { get; private set; }
	public IReadOnlyList<Schedule> Schedules => _schedules;

	public static Result<Institution> Create(string name, string shortName, string? description)
	{
		if (name.Length > MaxNameLength || name.Length < MinNameLength)
		{
			return Result.Failure<Institution>($"Название учреждения должно содержать от {MinNameLength} до {MaxNameLength} символов");
		}
		if (shortName.Length > MaxShortNameLength || shortName.Length < MinShortNameLength)
		{
			return Result.Failure<Institution>($"Короткое название учреждения должно содержать от {MinShortNameLength} до {MaxShortNameLength} символов");
		}
		if (description?.Length > MaxDescriptionLength)
		{
			return Result.Failure<Institution>($"Описание учреждения не может быть длиннее {MaxDescriptionLength} символов");
		}

		var result = new Institution(name, shortName, description);
		return result;
	}

	public void AddSchedule(Schedule schedule)
	{
		if (!_schedules.Any(x => x.Id == schedule.Id))
		{
			_schedules.Add(schedule);
		}
	}
}