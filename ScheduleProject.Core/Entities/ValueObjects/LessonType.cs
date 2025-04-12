using CSharpFunctionalExtensions;

namespace ScheduleProject.Core.Entities.ValueObjects;

public class LessonType : ValueObject
{
	public const int MaxValueLength = 32;

	private LessonType(string value)
	{
		Value = value;
	}
	
	public static LessonType LaboratoryWork => new ("Лабораторная работа");
	public static LessonType PracticalLesson => new ("Практическое занятие");
	public static LessonType Consultation => new ("Консультация");
	public static LessonType Lecture => new ("Лекция");
	public string Value { get; private set; }

	public static Result<LessonType> Create(string value)
	{
		value = value.Trim();

		if (value.Length > MaxValueLength)
		{
			return Result.Failure<LessonType>($"Тип занятия не может быть больше {MaxValueLength} символов");
		}

		return new LessonType(value);
	}

	public static List<string> GetValues()
	{
		return ["Лабораторная работа", "Практическое занятие", "Лекция", "Консультация"];
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}
