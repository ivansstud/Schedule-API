using CSharpFunctionalExtensions;
using ScheduleProject.Core.Entities.Abstractions;

#pragma warning disable CS8618 

namespace ScheduleProject.Core.Entities;

public class Institution : EntityBase
{
	public const int MaxNameLength = 128;
	public const int MaxShortNameLength = 32;
	public const int MaxDescriptionLength = 256;
	
	public const int MinNameLength = 16;
	public const int MinShortNameLength = 3;

	private readonly List<Schedule> _schedules = [];

	private Institution() { } // Для EF Core

	private Institution(string name, string shortName, string? description, long ownerId)
	{
		Name = name;
		ShortName = shortName;
		Description = description;
		OwnerId = ownerId;
	}

	public string Name { get; private set; }
	public string ShortName { get; private set; }
	public string? Description { get; private set; }
	public long OwnerId { get; private set; }
	public AppUser Owner { get; private set; }
	public IReadOnlyList<Schedule> Schedules => _schedules;

	public static Result<Institution> Create(string name, string shortName, string? description, long ownerId)
	{
		Result[] validationResults = [
			ValidateName(name),
			ValidateShortName(shortName),
			ValidateDescription(description),
		];

		if (validationResults.Any(x => x.IsFailure))
		{
			return Result.Failure<Institution>(validationResults.First(x => x.IsFailure).Error);
		}

		return new Institution(name, shortName, description, ownerId);
	}

	public void SetOwner(AppUser owner)
	{
		ArgumentNullException.ThrowIfNull(owner);

		Owner = owner;
		OwnerId = owner.Id;
	}

	public void AddSchedule(Schedule schedule)
	{
		ArgumentNullException.ThrowIfNull(schedule);

		if (!_schedules.Any(x => x.Id == schedule.Id))
		{
			_schedules.Add(schedule);
		}
	}

	public void RemoveSchedule(Schedule schedule)
	{
		ArgumentNullException.ThrowIfNull(schedule);

		if (_schedules.Any(x => x.Id == schedule.Id))
		{
			_schedules.Remove(schedule);
		}
	}

	public Result SetName(string name)
	{
		name = name.Trim();

		if (ValidateName(name).TryGetError(out string? error))
		{
			return Result.Failure(error);
		}

		Name = name;
		return Result.Success();
	}

	public Result SetShortName(string shortName)
	{
		shortName = shortName.Trim();

		if (ValidateShortName(shortName).TryGetError(out var error))
		{
			return Result.Failure(error);
		}

		ShortName = shortName;
		return Result.Success();
	}

	public Result SetDescription(string? description)
	{
		description = description?.Trim();

		if (ValidateDescription(description).TryGetError(out var error))
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

	private static Result ValidateShortName(string shortName)
	{
		if (shortName.Length > MaxShortNameLength || shortName.Length < MinShortNameLength)
		{
			return Result.Failure($"Аббривиатура учреждения должна содержать от {MinShortNameLength} до {MaxShortNameLength} символов");
		}

		return Result.Success();
	}

	private static Result ValidateDescription(string? description)
	{
		if (description?.Length > MaxDescriptionLength)
		{
			return Result.Failure<Institution>($"Описание учреждения не может быть длиннее {MaxDescriptionLength} символов");
		}

		return Result.Success();
	}
}