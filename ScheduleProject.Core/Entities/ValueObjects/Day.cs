using CSharpFunctionalExtensions;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities.ValueObjects;

public class Day : ValueObject
{
	public const int MaxLength = 3;

	public static readonly Day Monday = new ("Пн", 1);
	public static readonly Day Tuesday = new ("Вт", 2);
	public static readonly Day Wednesday = new ("Ср", 3);
	public static readonly Day Thursday = new ("Чт", 4);
	public static readonly Day Friday = new ("Пт", 5);
	public static readonly Day Saturday = new ("Сб", 6);
	public static readonly Day Sunday = new ("Вс", 7);

	private static readonly IEnumerable<Day> s_all = [Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday];

	private Day() { } // Для EF Core

	private Day(string name, int value)
	{
		Name = name;
		Value = value;
	}

	public string Name { get; }
	public int Value { get; }

	public static Day? FromName(string? name)
	{
		return s_all.FirstOrDefault(x => x.Name == name);
	}

	public static Day? FromValue(int? value)
	{
		return s_all.FirstOrDefault(x => x.Value == value);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}
