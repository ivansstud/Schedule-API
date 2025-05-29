using CSharpFunctionalExtensions;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities.ValueObjects;

public class ScheduleRole : ValueObject
{
	public const int MaxLength = 20;

	public static readonly ScheduleRole Member = new("Участник", 1);
	public static readonly ScheduleRole Creator = new("Создатель", 2);
	public static readonly ScheduleRole Moderator = new("Модератор", 3);

	private static readonly IEnumerable<ScheduleRole> s_all = [Member, Creator, Moderator];

	private ScheduleRole() { } // Для EF Core

	private ScheduleRole(string name, int value)
	{
		Name = name;
		Value = value;
	}

	public string Name { get; }
	public int Value { get; }

	public static ScheduleRole? FromName(string? name)
	{
		return s_all.FirstOrDefault(x => x.Name == name);
	}

	public static ScheduleRole? FromValue(int? value)
	{
		return s_all.FirstOrDefault(x => x.Value == value);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}
