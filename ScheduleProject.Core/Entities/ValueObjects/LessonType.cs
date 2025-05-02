using CSharpFunctionalExtensions;

namespace ScheduleProject.Core.Entities.ValueObjects;

public class LessonType : ValueObject
{
	public const int MaxLength = 32;

	public static readonly LessonType LaboratoryWork = new ("Лабораторная работа");
	public static readonly LessonType PracticalLesson = new("Практическое занятие");
	public static readonly LessonType Consultation = new("Консультация");
	public static readonly LessonType Lecture = new("Лекция");

	private LessonType(string name)
	{
		Name = name;
	}
	
	public string Name { get; private set; }

	public static Result<LessonType> Create(string name)
	{
		name = name.Trim();

		if (name.Length > MaxLength)
		{
			return Result.Failure<LessonType>($"Формат занятия не может быть длинее {MaxLength} символов");
		}

		return new LessonType(name);
	}

	public static List<string> GetValues()
	{
		return [LaboratoryWork.Name, PracticalLesson.Name, Consultation.Name, Lecture.Name];
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Name;
	}
}
