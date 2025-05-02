using CSharpFunctionalExtensions;

#pragma warning disable CS8618

namespace ScheduleProject.Core.Entities.ValueObjects;

public class ScheduleType : ValueObject
{
	public const int MaxLength = 20;

	public static readonly ScheduleType Custom = new("Оригинальное", 1);
	public static readonly ScheduleType University = new("Университет", 2);
	public static readonly ScheduleType College = new("Колледж", 3);
	public static readonly ScheduleType School = new("Школа", 4);

	private static readonly IEnumerable<ScheduleType> _all = [Custom, University, College, School];

	private ScheduleType() { }

	private ScheduleType(string value, int code)
	{
		Name = value;
		Value = code;
	}

	public string Name { get; }
	public int Value { get; }

	public static ScheduleType? FromName(string? name)
	{
		return _all.FirstOrDefault(x => x.Name == name);
	}

	public static ScheduleType? FromValue(int? value)
	{
		return _all.FirstOrDefault(x => x.Value == value);
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}
