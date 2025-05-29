using CSharpFunctionalExtensions;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities.ValueObjects;

public class ScheduleWeeksType : ValueObject
{
	public const int MaxLength = 30;

	public static readonly ScheduleWeeksType Cyclic = new("Цикличное", 1);
	public static readonly ScheduleWeeksType Permanent = new("Постоянное", 2);

	private static readonly IEnumerable<ScheduleWeeksType> s_all = [Cyclic, Permanent];

	private ScheduleWeeksType() { } // Для EF Core

	private ScheduleWeeksType(string value, int code)
	{
		Name = value;
		Value = code;
	}

	public string Name { get; }
	public int Value { get; }

	public static ScheduleWeeksType? FromName(string? name)
	{
		return s_all.FirstOrDefault(x => x.Name == name);
	}

	public static ScheduleWeeksType? FromValue(int? value)
	{
		return s_all.FirstOrDefault(x => x.Value == value);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}
